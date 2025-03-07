using System;
using System.IO;
using Aquiris.Ballistic.Game.Services;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Address.Steam;
using Aquiris.Ballistic.Network.Transmission.Events;
using Aquiris.Ballistic.Network.Transmission.Packet;
using Aquiris.Ballistic.Network.Transmission.Packet.Thrift;
using Aquiris.Ballistic.Network.Transmission.Steam;
using Aquiris.Ballistic.Network.Transport.Connection.Events;
using Aquiris.Ballistic.Network.Transport.Connection.Requests;
using Aquiris.Ballistic.Network.Transport.Connection.Responses;
using Aquiris.Ballistic.Network.Transport.Gameplay.State;
using Steamworks;
using Thrift.Protocol;
using UnityEngine;

namespace Aquiris.Ballistic.Network.Connection.Steam
{
	// Token: 0x02000356 RID: 854
	public sealed class SteamConnectionClientManager : ConnectionClientManager<ThriftPacket, CSteamID, TBase, ETransportChannel, NetworkAddress<CSteamID>, SteamClientTransmitter>
	{
		// Token: 0x06001276 RID: 4726 RVA: 0x0000EEAE File Offset: 0x0000D0AE
		public SteamConnectionClientManager(SteamHostInfo steamHostInfo)
			: base(new SteamClientTransmitter(steamHostInfo), ETransportChannel.KEEP_ALIVE, ETransportChannel.CONNECTION_MANAGEMENT)
		{
			this._authTicket = new byte[1024];
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0000EECE File Offset: 0x0000D0CE
		internal override void JoinGame(EClientMode mode, string password)
		{
			SteamCallbacks.GetAuthSessionTicketResponse_t.RegisterCallback(new Action<GetAuthSessionTicketResponse_t>(this.OnGetAuthTicketResponse));
			this._clientMode = mode;
			this._password = password;
			this._authTicketHandler = SteamUser.GetAuthSessionTicket(this._authTicket, this._authTicket.Length, ref this._authTicketLength);
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00066950 File Offset: 0x00064B50
		private void OnGetAuthTicketResponse(GetAuthSessionTicketResponse_t data)
		{
			bool flag = data.m_eResult == 1;
			Debug.Log("CLIENT GetAuthTicket " + data.m_eResult.ToString());
			if (flag)
			{
				base.JoinGame(this._clientMode, this._password);
			}
			else
			{
				Debug.LogWarning("Can't get steam auth ticket: " + data.m_eResult.ToString());
				this.OnConnectionFailed();
			}
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x0000EF0E File Offset: 0x0000D10E
		internal override void LeaveGame(CurrentMatchService.ConnectionState motivation)
		{
			base.LeaveGame(motivation);
			SteamUser.CancelAuthTicket(this._authTicketHandler);
			SteamCallbacks.GetAuthSessionTicketResponse_t.UnregisterCallback(new Action<GetAuthSessionTicketResponse_t>(this.OnGetAuthTicketResponse));
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x000669D0 File Offset: 0x00064BD0
		protected override ThriftPacket getKeepAliveRequestPacket()
		{
			return new ThriftPacket
			{
				TransportChannel = ETransportChannel.KEEP_ALIVE,
				TransportObject = new KeepAliveRequest(),
				TransportType = ETransportType.Reliable
			};
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00066A00 File Offset: 0x00064C00
		protected override ThriftPacket getKeepAliveResponsePacket()
		{
			return new ThriftPacket
			{
				TransportChannel = ETransportChannel.KEEP_ALIVE,
				TransportObject = new KeepAliveResponse(),
				TransportType = ETransportType.Reliable
			};
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0000EF33 File Offset: 0x0000D133
		protected override bool isKeepAliveRequestPacket(ThriftPacket packet)
		{
			return packet.TransportObject is KeepAliveRequest;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0000EF43 File Offset: 0x0000D143
		protected override bool isKeepAliveResponsePacket(ThriftPacket packet)
		{
			return packet.TransportObject is KeepAliveResponse;
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00066A30 File Offset: 0x00064C30
		protected override ThriftPacket GetJoinGameRequestPacket(EClientMode mode, string password)
		{
			JoinGameRequest joinGameRequest = new JoinGameRequest
			{
				AuthTicket = new byte[this._authTicketLength],
				UserAlias = ((!string.IsNullOrEmpty(File.ReadAllText(Application.dataPath + "/NameTag.txt").Trim())) ? File.ReadAllText(Application.dataPath + "/NameTag.txt") : SteamFriends.GetPersonaName()),
				Version = BallisticVersion.VERSION,
				ClientMode = mode,
				Password = password
			};
			ThriftPacket thriftPacket = new ThriftPacket();
			thriftPacket.TransportChannel = ETransportChannel.CONNECTION_MANAGEMENT;
			thriftPacket.TransportObject = joinGameRequest;
			thriftPacket.TransportType = ETransportType.Reliable;
			Buffer.BlockCopy(this._authTicket, 0, joinGameRequest.AuthTicket, 0, (int)this._authTicketLength);
			return thriftPacket;
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00066AE4 File Offset: 0x00064CE4
		protected override ThriftPacket GetLeaveGameRequestPacket()
		{
			return new ThriftPacket
			{
				TransportChannel = ETransportChannel.CONNECTION_MANAGEMENT,
				TransportObject = new LeaveGameRequest(),
				TransportType = ETransportType.Reliable
			};
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x0000EF53 File Offset: 0x0000D153
		protected override bool IsJoinGameEventPacket(ThriftPacket packet)
		{
			return packet.TransportObject is JoinGameEvent;
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x0000EF63 File Offset: 0x0000D163
		protected override bool IsLeaveGameEventPacket(ThriftPacket packet)
		{
			return packet.TransportObject is LeaveGameEvent;
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0000EF73 File Offset: 0x0000D173
		protected override bool IsFailConnectionResponsePacket(ThriftPacket packet)
		{
			return packet.TransportObject is FailConnectionResponse;
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00066B14 File Offset: 0x00064D14
		protected override NetworkAddress<CSteamID> GetConnectingClient(ThriftPacket packet)
		{
			if (packet.TransportObject is JoinGameEvent)
			{
				JoinGameEvent joinGameEvent = (JoinGameEvent)packet.TransportObject;
				return SteamClientFactory.GetClient(new CSteamID((ulong)joinGameEvent.User));
			}
			if (packet.TransportObject is LeaveGameEvent)
			{
				LeaveGameEvent leaveGameEvent = (LeaveGameEvent)packet.TransportObject;
				return SteamClientFactory.GetClient(new CSteamID((ulong)leaveGameEvent.User));
			}
			return null;
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00002A31 File Offset: 0x00000C31
		protected override void OnClientConnected(NetworkAddress<CSteamID> client)
		{
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0000EF83 File Offset: 0x0000D183
		protected override void OnClientDisconnected(NetworkAddress<CSteamID> client)
		{
			if (client is SteamClientInfo)
			{
				SteamClientFactory.FreeClient(client.Info);
			}
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00002A31 File Offset: 0x00000C31
		protected override void OnConnectionEstablished()
		{
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00002A31 File Offset: 0x00000C31
		protected override void OnConnectionFailed()
		{
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0000EF9B File Offset: 0x0000D19B
		internal bool IsValid(PacketReceivedEventArgs<ThriftPacket, CSteamID, TBase, ETransportChannel, NetworkAddress<CSteamID>> p_packetArgs)
		{
			return p_packetArgs.Sender == this.m_transmitter.BasicHost || this.m_knownClientList.ContainsValue(p_packetArgs.Sender);
		}

		// Token: 0x040016E3 RID: 5859
		private EClientMode _clientMode;

		// Token: 0x040016E4 RID: 5860
		private string _password;

		// Token: 0x040016E5 RID: 5861
		private HAuthTicket _authTicketHandler;

		// Token: 0x040016E6 RID: 5862
		private readonly byte[] _authTicket;

		// Token: 0x040016E7 RID: 5863
		private uint _authTicketLength;
	}
}

using System;
using Aquiris.Ballistic.Network.Address;
using Aquiris.Ballistic.Network.Address.Steam;
using Aquiris.Ballistic.Network.Transmission.Mapper.Thrift;
using Aquiris.Ballistic.Network.Transmission.Packet;
using Aquiris.Ballistic.Network.Transmission.Packet.Thrift;
using Aquiris.Ballistic.Utils;
using Steamworks;
using Thrift.Protocol;
using UnityEngine;

namespace Aquiris.Ballistic.Network.Transmission.Steam
{
	// Token: 0x020003CC RID: 972
	public class SteamClientTransmitter : BasicClientTransmitter<ThriftPacket, CSteamID, TBase, ETransportChannel, NetworkAddress<CSteamID>>
	{
		// Token: 0x0600155F RID: 5471 RVA: 0x00010750 File Offset: 0x0000E950
		public SteamClientTransmitter(SteamHostInfo p_host)
			: base(p_host)
		{
			this.m_packetMapper = new ThriftByteArrayPacketMapper();
			SteamCallbacks.P2PSessionConnectFail_t.RegisterCallback(new Action<P2PSessionConnectFail_t>(this.OnP2PSessionConnectFail), false);
			SteamCallbacks.P2PSessionRequest_t.RegisterCallback(new Action<P2PSessionRequest_t>(this.OnP2PSessionRequest), false);
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x00010788 File Offset: 0x0000E988
		private void OnP2PSessionRequest(P2PSessionRequest_t pCallback)
		{
			Debug.Log("[OnP2PSessionRequest]:" + pCallback.m_steamIDRemote);
			SteamNetworking.AcceptP2PSessionWithUser(pCallback.m_steamIDRemote);
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x00073EA4 File Offset: 0x000720A4
		private void OnP2PSessionConnectFail(P2PSessionConnectFail_t pCallback)
		{
			if (pCallback.m_steamIDRemote.m_SteamID == base.BasicHost.Info.m_SteamID)
			{
				object[] array = new object[4];
				array[0] = "[OnHostSessionConnectFail]:";
				array[1] = pCallback.m_steamIDRemote;
				array[2] = ":";
				int num = 3;
				EP2PSessionError eP2PSessionError = pCallback.m_eP2PSessionError;
				array[num] = eP2PSessionError.ToString();
				Debug.LogWarning(string.Concat(array));
			}
			else
			{
				object[] array2 = new object[4];
				array2[0] = "[OnP2PSessionConnectFail]:";
				array2[1] = pCallback.m_steamIDRemote;
				array2[2] = ":";
				int num2 = 3;
				EP2PSessionError eP2PSessionError2 = pCallback.m_eP2PSessionError;
				array2[num2] = eP2PSessionError2.ToString();
				Debug.LogWarning(string.Concat(array2));
			}
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00073F68 File Offset: 0x00072168
		internal override bool SendPacket(ThriftPacket p_packet, NetworkAddress<CSteamID> p_networkAddress)
		{
			byte[] array = this.m_packetMapper.compressPacket(p_packet);
			EP2PSend ep2PSend = 0;
			ETransportType transportType = p_packet.TransportType;
			if (transportType != ETransportType.Reliable)
			{
				if (transportType == ETransportType.ReliableOrdered)
				{
					ep2PSend = 3;
				}
			}
			else
			{
				ep2PSend = 2;
			}
			return SteamNetworking.SendP2PPacket(p_networkAddress.Info, array, (uint)array.Length, ep2PSend, 0);
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00073FC0 File Offset: 0x000721C0
		internal override void UpdateTransmitter(float p_deltaTime)
		{
			uint num = 0U;
			while (SteamNetworking.IsP2PPacketAvailable(ref num, 0))
			{
				byte[] buffer = ArrayPool<byte>.GetBuffer((int)num);
				CSteamID csteamID;
				if (!SteamNetworking.ReadP2PPacket(buffer, num, ref num, ref csteamID, 0))
				{
					Debug.LogError("Could not read p2p packet");
					break;
				}
				ThriftPacket thriftPacket = this.m_packetMapper.uncompressPacket(buffer);
				NetworkAddress<CSteamID> networkAddress;
				if (csteamID == base.BasicHost.Info)
				{
					networkAddress = base.BasicHost;
				}
				else
				{
					networkAddress = SteamClientFactory.GetClient(csteamID);
				}
				base.receivePacket(thriftPacket, networkAddress);
				ArrayPool<byte>.FreeBuffer(buffer);
			}
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x000107B2 File Offset: 0x0000E9B2
		internal override void Close()
		{
			SteamCallbacks.P2PSessionConnectFail_t.UnregisterCallback(new Action<P2PSessionConnectFail_t>(this.OnP2PSessionConnectFail));
			SteamCallbacks.P2PSessionRequest_t.UnregisterCallback(new Action<P2PSessionRequest_t>(this.OnP2PSessionRequest));
			SteamNetworking.CloseP2PSessionWithUser(base.BasicHost.Info);
			SteamClientFactory.FreeAll();
		}

		// Token: 0x04001892 RID: 6290
		private ThriftByteArrayPacketMapper m_packetMapper;
	}
}

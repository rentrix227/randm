using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aquiris.Ballistic.Network.Transport.Connection.Requests;
using Aquiris.Ballistic.Network.Transport.Connection.Responses;
using Thrift.Protocol;

namespace Aquiris.Ballistic.Network.Transmission.Mapper.Thrift
{
	// Token: 0x020003C6 RID: 966
	internal class TypeByteGenerator
	{
		// Token: 0x06001548 RID: 5448 RVA: 0x00073BBC File Offset: 0x00071DBC
		private TypeByteGenerator()
		{
			this.valuesByClass.Add(typeof(JoinGameRequest), 0);
			this.classByValue.Add(0, typeof(JoinGameRequest));
			this.valuesByClass.Add(typeof(FailConnectionResponse), 1);
			this.classByValue.Add(1, typeof(FailConnectionResponse));
			IEnumerable<Type> enumerable = from t in Assembly.GetExecutingAssembly().GetTypes()
				where typeof(TBase).IsAssignableFrom(t)
				orderby t.Name
				select t;
			sbyte b = 2;
			foreach (Type type in enumerable)
			{
				if (!(type == typeof(JoinGameRequest)) && !(type == typeof(FailConnectionResponse)))
				{
					this.valuesByClass.Add(type, b);
					this.classByValue.Add(b, type);
					b = (sbyte)((int)b + 1);
				}
			}
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x00010662 File Offset: 0x0000E862
		internal static TypeByteGenerator GetInstance()
		{
			if (TypeByteGenerator.instance == null)
			{
				TypeByteGenerator.instance = new TypeByteGenerator();
			}
			return TypeByteGenerator.instance;
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0001067D File Offset: 0x0000E87D
		internal sbyte GetByteFromType(TBase p_object)
		{
			if (this.valuesByClass != null && this.valuesByClass.ContainsKey(p_object.GetType()))
			{
				return this.valuesByClass[p_object.GetType()];
			}
			return -1;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x00073D28 File Offset: 0x00071F28
		internal TBase GetTypeFromByte(sbyte p_object)
		{
			if (this.objectByValue.ContainsKey(p_object))
			{
				return (TBase)this.objectByValue[p_object];
			}
			if (this.classByValue.ContainsKey(p_object))
			{
				TBase tbase = (TBase)Activator.CreateInstance(this.classByValue[p_object]);
				this.objectByValue.Add(p_object, tbase);
				return tbase;
			}
			return null;
		}

		// Token: 0x04001878 RID: 6264
		private static TypeByteGenerator instance;

		// Token: 0x04001879 RID: 6265
		internal Dictionary<Type, sbyte> valuesByClass = new Dictionary<Type, sbyte>();

		// Token: 0x0400187A RID: 6266
		internal Dictionary<sbyte, Type> classByValue = new Dictionary<sbyte, Type>();

		// Token: 0x0400187B RID: 6267
		internal Dictionary<sbyte, object> objectByValue = new Dictionary<sbyte, object>();
	}
}

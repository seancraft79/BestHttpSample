#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
    public class DerInteger
        : Asn1Object
    {
        public const string AllowUnsafeProperty = "BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.AllowUnsafeInteger";

        internal static bool AllowUnsafe()
        {
            string allowUnsafeValue = BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Platform.GetEnvironmentVariable(AllowUnsafeProperty);
            return allowUnsafeValue != null && BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Platform.EqualsIgnoreCase("true", allowUnsafeValue);
        }

        private readonly byte[] bytes;

        /**
         * return an integer from the passed in object
         *
         * @exception ArgumentException if the object cannot be converted.
         */
        public static DerInteger GetInstance(
            object obj)
        {
            if (obj == null || obj is DerInteger)
            {
                return (DerInteger)obj;
            }

            throw new ArgumentException("illegal object in GetInstance: " + BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Platform.GetTypeName(obj));
        }

        /**
         * return an Integer from a tagged object.
         *
         * @param obj the tagged object holding the object we want
         * @param isExplicit true if the object is meant to be explicitly
         *              tagged false otherwise.
         * @exception ArgumentException if the tagged object cannot
         *               be converted.
         */
        public static DerInteger GetInstance(
            Asn1TaggedObject	obj,
            bool				isExplicit)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

			Asn1Object o = obj.GetObject();

			if (isExplicit || o is DerInteger)
			{
				return GetInstance(o);
			}

			return new DerInteger(Asn1OctetString.GetInstance(o).GetOctets());
        }

		public DerInteger(
            int value)
        {
            bytes = BigInteger.ValueOf(value).ToByteArray();
        }

		public DerInteger(
            BigInteger value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

			bytes = value.ToByteArray();
        }

		public DerInteger(
            byte[] bytes)
        {
            if (bytes.Length > 1)
            {
                if ((bytes[0] == 0 && (bytes[1] & 0x80) == 0)
                    || (bytes[0] == (byte)0xff && (bytes[1] & 0x80) != 0))
                {
                    if (!AllowUnsafe())
                        throw new ArgumentException("malformed integer");
                }
            }
            this.bytes = Arrays.Clone(bytes);
        }

		public BigInteger Value
        {
            get { return new BigInteger(bytes); }
        }

		/**
         * in some cases positive values Get crammed into a space,
         * that's not quite big enough...
         */
        public BigInteger PositiveValue
        {
            get { return new BigInteger(1, bytes); }
        }

        internal override void Encode(
            DerOutputStream derOut)
        {
            derOut.WriteEncoded(Asn1Tags.Integer, bytes);
        }

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(bytes);
        }

		protected override bool Asn1Equals(
			Asn1Object asn1Object)
		{
			DerInteger other = asn1Object as DerInteger;

			if (other == null)
				return false;

			return Arrays.AreEqual(this.bytes, other.bytes);
        }

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
#pragma warning restore
#endif

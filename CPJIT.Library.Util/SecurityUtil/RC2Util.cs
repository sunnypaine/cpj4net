using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CPJIT.Library.Util.SecurityUtil
{
    /// <summary>
    /// 提供对称的RC2加密。
    /// </summary>
    public class RC2Util
    {
        #region 私有变量
        /// <summary>
        /// RC2加密对象
        /// </summary>
        private RC2 rc2;
        #endregion


        #region 公共属性
        /// <summary>
        /// 公钥
        /// </summary>
        public string Key
        { get; set; }

        /// <summary>
        /// 私钥
        /// </summary>
        public string IV;
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用默认参数创建实例
        /// </summary>
        public RC2Util()
        { }

        /// <summary>
        /// 使用指定参数创建实例
        /// </summary>
        /// <param name="key">公钥</param>
        public RC2Util(string key)
        {
            this.rc2 = new RC2CryptoServiceProvider();
            this.Key = key;
            this.IV = "x00x01x02x08x99_QAZwsxEDCrfvTGByhnUJMikOLp_com.cpjit.library";
        }

        /// <summary>
        /// 使用指定参数创建实例。
        /// </summary>
        /// <param name="key">公钥</param>
        /// <param name="iv">私钥</param>
        public RC2Util(string key, string iv)
        {
            this.rc2 = new RC2CryptoServiceProvider();
            this.Key = key;
            this.IV = iv;
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            this.rc2.GenerateKey();
            byte[] bytTemp = this.rc2.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);
            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength, ' ');
            }
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        private byte[] GetLegalIV()
        {
            string sTemp = IV;
            this.rc2.GenerateIV();
            byte[] bytTemp = this.rc2.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength, ' ');
            }
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="Source">待加密的串</param>
        /// <returns>经过加密的串</returns>
        public string Encrypt(string Source)
        {
            try
            {
                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
                MemoryStream ms = new MemoryStream();
                this.rc2.Key = GetLegalKey();
                this.rc2.IV = GetLegalIV();
                ICryptoTransform encrypto = this.rc2.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                throw new Exception("在文件加密的时候出现错误。", ex);
            }
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Source">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public string Decrypt(string Source)
        {
            try
            {
                byte[] bytIn = Convert.FromBase64String(Source);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                this.rc2.Key = GetLegalKey();
                this.rc2.IV = GetLegalIV();
                ICryptoTransform encrypto = this.rc2.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("在文件解密的时候出现错误。", ex);
            }
        }
        #endregion
    }
}

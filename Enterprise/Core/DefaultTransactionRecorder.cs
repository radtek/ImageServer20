using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.Enterprise.Core
{
    public class DefaultTransactionRecorder : ITransactionRecorder
    {
        private string _transactionName;

        public DefaultTransactionRecorder(string transactionName)
        {
            _transactionName = transactionName;
        }

        #region ITransactionRecorder Members

        public TransactionRecord CreateTransactionRecord(ICollection<EntityChange> changeSet)
        {
            return new TransactionRecord(_transactionName, WriteXml(changeSet));
        }

        #endregion

        private string WriteXml(ICollection<EntityChange> changeSet)
        {
            StringWriter sw = new StringWriter();
            using (XmlTextWriter writer = new XmlTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                WriteXml(writer, changeSet);
                return sw.ToString();
            }
        }

        private void WriteXml(XmlWriter writer, ICollection<EntityChange> changeSet)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("transaction-record");
            foreach (EntityChange entityChange in changeSet)
            {
                writer.WriteStartElement("action");
                writer.WriteAttributeString("type", entityChange.ChangeType.ToString());
                writer.WriteAttributeString("class", EntityRefUtils.GetClass(entityChange.EntityRef).FullName);
                writer.WriteAttributeString("oid", EntityRefUtils.GetOID(entityChange.EntityRef).ToString());
                writer.WriteAttributeString("version", EntityRefUtils.GetVersion(entityChange.EntityRef).ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

    }
}

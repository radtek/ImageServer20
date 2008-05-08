#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System.Collections.Generic;
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.ImageServer.Model;

namespace ClearCanvas.ImageServer.Rules.StudyDeleteAction
{
	[ExtensionOf(typeof(SampleRuleExtensionPoint))]
	public class AgeBasedDeleteSample : ISampleRule
	{
		private readonly IList<ServerRuleApplyTimeEnum> _applyTime = new List<ServerRuleApplyTimeEnum>();

		public AgeBasedDeleteSample()
		{
			_applyTime.Add(ServerRuleApplyTimeEnum.GetEnum("SopProcessed"));
		}
		public string Name
		{
			get { return "AgeBasedDelete"; }
		}
		public string Description
		{
			get { return "Age Based Delete"; }
		}

		public ServerRuleTypeEnum Type
		{
			get { return ServerRuleTypeEnum.GetEnum("StudyDelete"); }
		}

		public IList<ServerRuleApplyTimeEnum> ApplyTimeList
		{
			get { return _applyTime; }
		}

		public XmlDocument Rule
		{
			get
			{
				XmlDocument doc = new XmlDocument();
				XmlNode node = doc.CreateElement("rule");
				doc.AppendChild(node);
				XmlElement conditionNode = doc.CreateElement("condition");
				node.AppendChild(conditionNode);
				conditionNode.SetAttribute("expressionLanguage", "dicom");
				XmlNode actionNode = doc.CreateElement("action");
				node.AppendChild(actionNode);

				XmlElement dicomAgeNode = doc.CreateElement("dicom-age-less-than");
				dicomAgeNode.SetAttribute("test", "$PatientsBirthDate");
				dicomAgeNode.SetAttribute("units", "years");
				dicomAgeNode.SetAttribute("refValue", "21");
				conditionNode.AppendChild(dicomAgeNode);

				XmlElement studyDelete = doc.CreateElement("study-delete");
				studyDelete.SetAttribute("time", "21");
				studyDelete.SetAttribute("timeUnits", "patientAge");
				actionNode.AppendChild(studyDelete);
				return doc;
			}
		}
	}

	[ExtensionOf(typeof(SampleRuleExtensionPoint))]
	public class MultiTagDeleteSample : ISampleRule
	{
		private readonly IList<ServerRuleApplyTimeEnum> _applyTime = new List<ServerRuleApplyTimeEnum>();

		public MultiTagDeleteSample()
		{
			_applyTime.Add(ServerRuleApplyTimeEnum.GetEnum("SopProcessed"));
		}
		public string Name
		{
			get { return "TagBasedDelete"; }
		}
		public string Description
		{
			get { return "Tag Based Delete"; }
		}

		public ServerRuleTypeEnum Type
		{
			get { return ServerRuleTypeEnum.GetEnum("StudyDelete"); }
		}

		public IList<ServerRuleApplyTimeEnum> ApplyTimeList
		{
			get { return _applyTime; }
		}

		public XmlDocument Rule
		{
			get
			{
				XmlDocument doc = new XmlDocument();
				XmlNode node = doc.CreateElement("rule");
				doc.AppendChild(node);
				XmlElement conditionNode = doc.CreateElement("condition");
				node.AppendChild(conditionNode);
				conditionNode.SetAttribute("expressionLanguage", "dicom");
				XmlNode actionNode = doc.CreateElement("action");
				node.AppendChild(actionNode);

				XmlElement andNode = doc.CreateElement("and");
				conditionNode.AppendChild(andNode);
				XmlElement equalNode = doc.CreateElement("equal");
				equalNode.SetAttribute("test", "$Modality");
				equalNode.SetAttribute("refValue", "MR");
				andNode.AppendChild(equalNode);
				equalNode = doc.CreateElement("regex");
				equalNode.SetAttribute("test", "$PatientId");
				equalNode.SetAttribute("pattern", "1");
				andNode.AppendChild(equalNode);

				XmlElement studyDelete = doc.CreateElement("study-delete");
				studyDelete.SetAttribute("time", "10");
				studyDelete.SetAttribute("timeUnits", "weeks");
				actionNode.AppendChild(studyDelete);

				return doc;
			}
		}
	}
}

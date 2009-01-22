using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Web.Common.Data;

namespace ClearCanvas.ImageServer.Web.Application.Controls
{
    public partial class AlertIndicator : System.Web.UI.UserControl
    {
        protected IList<Alert> alerts;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            AlertController controller = new AlertController();
            AlertSelectCriteria criteria = new AlertSelectCriteria();

            criteria.AlertLevelEnum.EqualTo(AlertLevelEnum.Critical);
            criteria.InsertTime.SortDesc(1);

            AlertsCount.Text = controller.GetAlertsCount(criteria).ToString();

            alerts = controller.GetAlerts(criteria);

            if (alerts.Count > 0) {

                int rows = 0;
                foreach (Alert alert in alerts)
                {
                    TableRow alertRow = new TableRow();

                    alertRow.Attributes.Add("class", "AlertTableCell");

                    TableCell component = new TableCell();
                    TableCell source = new TableCell();
                    TableCell description = new TableCell();

                    description.Wrap = false;

                    component.Text = alert.Component;
                    component.Wrap = false;
                    source.Text = alert.Source;
                    source.Wrap = false;

                    string content = alert.Content.GetElementsByTagName("Message").Item(0).InnerText;
                    description.Text = content.Length < 50 ? content : content.Substring(0, 50);
                    description.Text += " ...";
                    description.Wrap = false;

                    alertRow.Cells.Add(component);
                    alertRow.Cells.Add(source);
                    alertRow.Cells.Add(description);

                    AlertTable.Rows.Add(alertRow);

                    rows++;
                    if (rows == 5) break;
                }
            }
        }
    }
}
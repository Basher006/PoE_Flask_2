using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskSetup
{
    internal static class GroupBoxPreload
    {
        internal static System.Windows.Forms.ComboBox[] groupBoxes;

        static GroupBoxPreload()
        {
            groupBoxes = new System.Windows.Forms.ComboBox[5];
            for (int i = 0; i < 5; i++)
            {
                groupBoxes[i] = new System.Windows.Forms.ComboBox();
                groupBoxes[i].Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                groupBoxes[i].DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                groupBoxes[i].FormattingEnabled = true;
                groupBoxes[i].Items.AddRange(new object[] {
                    "0",
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8",
                    "9",
                    "NumPad 0",
                    "NumPad 1",
                    "NumPad 2",
                    "NumPad 3",
                    "NumPad 4",
                    "NumPad 5",
                    "NumPad 6",
                    "NumPad 7",
                    "NumPad 8",
                    "NumPad 9"});
                groupBoxes[i].Location = new System.Drawing.Point(6, 130);
                groupBoxes[i].Name = "f4_Keys";
                groupBoxes[i].Size = new System.Drawing.Size(90, 21);
                groupBoxes[i].TabIndex = 36;
            }


        }
    }
}

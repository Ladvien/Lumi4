using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Lumi4.UI
{
    class DialogBoxYesOrNo
    {


        public async Task<bool> ShowDialogBox(string message)
        {
            var dialog = new MessageDialog(message);

            var yesCommand = new UICommand("Yes") { Id = 0 };
            var noCommand = new UICommand("No") { Id = 1 };
            dialog.Commands.Add(yesCommand);
            dialog.Commands.Add(noCommand);

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
            if(result == yesCommand) { return true; } else { return false; }
        }
    }
}

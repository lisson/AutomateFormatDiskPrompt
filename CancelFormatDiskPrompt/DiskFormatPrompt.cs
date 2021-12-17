using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace CancelFormatDiskPrompt
{
    class DiskFormatPrompt
    {
        // Clicks the Format button
        public static void Format()
        {

        }

        public static void Cancel()
        {
            int retries = 0;

            var DialogCondition = new AndCondition(
                new PropertyCondition(AutomationElement.NameProperty, "Microsoft Windows"),
                new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "Dialog"));

            while (retries < 10)
            {
                //Finds if automation element is available 
                var dialogs = AutomationElement.RootElement.FindAll(TreeScope.Children, DialogCondition);
                foreach(AutomationElement d in dialogs)
                {
                    InvokeCancelButtonElements(d);
                }
                retries++;
                Thread.Sleep(100);
            }
            
            // Prompt can also be the descendent of "File Explorer"
            retries = 0;
            while (retries < 10)
            {
                //Finds if automation element is available 
                var FileExplorer = AutomationElement.RootElement.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "File Explorer"));
                foreach (AutomationElement f in FileExplorer)
                {
                    InvokeCancelButtonElements(f);
                }
                retries++;
                Thread.Sleep(100);
            }
        }

        public static void InvokeCancelButtonElements(AutomationElement element)
        {
            TreeWalker walker = TreeWalker.ControlViewWalker;
            var PromptCondition = new PropertyCondition(AutomationElement.NameProperty, "Do you want to format it?");
            var prompt = element.FindFirst(TreeScope.Descendants, PromptCondition);
            if(prompt == null)
            {
                return;
            }
            var aeParent = walker.GetParent(prompt);
            // Impossible but let's check anyways
            if (aeParent == null)
            {
                return;
            }
            var cancelButton = aeParent.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.AutomationIdProperty, "CommandButton_2"));
            // But how ?
            if (cancelButton == null)
            {
                return;
            }
            DiskFormatPrompt.InvokeAutomationElement(cancelButton);
        }

        public static void InvokeAutomationElement(AutomationElement element)
        {
            if(element == null)
            {
                return;
            }
            // KAEL THE
            var Invoker = (InvokePattern)(element.GetCurrentPattern(InvokePattern.Pattern));
            Invoker.Invoke(); //Ultimate
        }
    }
}

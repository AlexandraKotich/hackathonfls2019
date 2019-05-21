using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

namespace ConsoleApp
{
    class Parser
    {
        public static char[] Separators = { ' ', ',', '.', '\n', '\"' };
        enum endofverbs { ишь, ть, вь, или, ели, али, уй, уйте, ьте, ни };
        enum addroots { лож, лаг, надбав, добав, ключ, велич, полн, прибав, присоед, плюс };
        enum deleteroots { удал, убер, убра, меньш, вын };
        enum deleteallroots { очист };
        enum readallroots { покаж, вывест, вывед, показ, предст };
        public ChatResponse TryParse(string userText)
        {
            ChatResponse chatResponse = new ChatResponse();
            string actionItem = "";

            try { var words = userText.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

                string actionItem = ActionItem(words);
                chatResponse.userAction = actionItem;
            }
            catch (Exception exc)
            {
                chatResponse.userAction = Add;
            }
            int countItem = CountItem(words);
            //some analysis of userText
            //...
            return chatResponse(chatResponse.userAction, "someItem", countItem, new Unit("kg"));
        }

        public string ActionItem (string[] words)
        {
            foreach (string stringwrite in words)
            {
                if (stringwrite.Contains(endofverbs))
                {
                    if (stringwrite.Contains(addroots))
                    {
                        chatResponse.userAction = Add;
                    }
                    else if (stringwrite.Contains(deleteroots))
                    {
                        chatResponse.userAction = deleteLast;
                    }
                    else if (stringwrite.Contains(deleteallroots))
                    {
                        chatResponse.userAction = clearAll;
                    }
                    else if (stringwrite.Contains(readallroots))
                    {
                        chatResponse.userAction = readAll;
                    }
                }
                else
                {
                    chatResponse.userAction = Add;
                }
            }
        }

        public double CountItem (string[] words)
        {
            double count = 1;
            foreach (string countstring in words)
            try {
                    Int32.TryParse(countstring, out count);
            }
                catch (Exception exc)
                {

                }
            return count; 
        }
        public Unit UnitItem (string[] words)
        {
            Unit unit = new Unit();

            return unit;
        }
    }
}

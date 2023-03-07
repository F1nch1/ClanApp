using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Threading;

namespace ClanCreatorApp
{
    public class Persuasion
    {
        

        public bool PersuadeCat(Cat catToPersuade, Cat playerCat, Clan clan, DataTable table)
        {
            bool success1;
            bool success2;
            bool success3;
            //Console.Clear();
            success1 = Introduction(catToPersuade, playerCat, clan, table);

            if(success1)
            {
                Console.Clear();
                success2 = Invitation(catToPersuade, playerCat, clan, table);
                if(success2)
                {
                    Console.Clear();
                    success3 = Persuade(catToPersuade, playerCat, clan, table);
                    MainFile.PressEToContinue();
                    return true;
                }
            }
            Console.Clear();
            return false;
        }
        public static float GetInteractionScore(Cat catToPersuade, Cat playerCat, Clan clan, DataTable table)
        {
            float interactionScore = 0;
            interactionScore += float.Parse(table.Rows[Utils.GetRow(Defs.GetPersonalityType(catToPersuade.personality[0]), table)][table.Columns[Utils.GetColumn(Defs.GetPersonalityType(playerCat.personality[0]), table)].ColumnName].ToString());
            interactionScore += float.Parse(table.Rows[Utils.GetRow(Defs.GetPersonalityType(catToPersuade.personality[1]), table)][table.Columns[Utils.GetColumn(Defs.GetPersonalityType(playerCat.personality[0]), table)].ColumnName].ToString());
            interactionScore += float.Parse(table.Rows[Utils.GetRow(Defs.GetPersonalityType(catToPersuade.personality[0]), table)][table.Columns[Utils.GetColumn(Defs.GetPersonalityType(playerCat.personality[1]), table)].ColumnName].ToString());
            interactionScore += float.Parse(table.Rows[Utils.GetRow(Defs.GetPersonalityType(catToPersuade.personality[1]), table)][table.Columns[Utils.GetColumn(Defs.GetPersonalityType(playerCat.personality[1]), table)].ColumnName].ToString());
            interactionScore /= 4;
            return interactionScore;
        }
        public bool Introduction(Cat catToPersuade, Cat playerCat, Clan clan, DataTable table)
        {
            Console.WriteLine("You encounter a cat on your territory. They appear " + catToPersuade.personality[0].ToLower() + " and " + catToPersuade.personality[1].ToLower() + ".");
            Console.WriteLine("\n[0]: Hello? I won’t hurt you, I’m a friend.\n[1]: Oh, hey there! How are you?\n[2]: What do you think you’re doing?!\n[3]: Hey, kid. You’re on my land. Wanna talk about it?\n[4]: Nevermind, I’ve got to be going.");
            string checkIntro = "";
            while (!checkIntro.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkIntro) || int.Parse(checkIntro) > 4)
            {
                checkIntro = Console.ReadLine();
                
            }
            int introNumber = int.Parse(checkIntro);
            int row = -1;
            switch(introNumber)
            {
                case 0:
                    //Kind
                    row = 6;
                    break;
                case 1:
                    row = 5;
                    break;
                case 2:
                    row = 3;
                    break;
                case 3:
                    row = 1;
                    break;
                case 4:
                    return false;
            }
            float replyScore = 0;
            
            for(int i = 0; i < 2; i++)
            {
                replyScore += float.Parse(table.Rows[Utils.GetRow(Defs.GetPersonalityType(catToPersuade.personality[i]), table)][table.Columns[row].ColumnName].ToString());
            }
            replyScore /= 2;
            //Console.WriteLine("Reply score: " + replyScore);
            float interactionScore = GetInteractionScore(catToPersuade, playerCat, clan, table);
            //Console.WriteLine("Interaction score: " + interactionScore);
            float score = replyScore * interactionScore;
            //Console.WriteLine("Score: " + score);
            if (score >= 1)
            {
                Console.WriteLine(IntroResponse(introNumber));
                return true;
            }
            else
            {
                Console.WriteLine("The strange cat's ears flatten before they flee into the distance. Oh well.");
                MainFile.PressEToContinue();
                return false;
            }
        }

        public string IntroResponse(int intro)
        {
            switch(intro)
            {
                case 0:
                    return "The cat's ears perk up as they cautiously approach.";
                case 1:
                    return "The cat turns to look at you, whiskers twitching happily as they trot over to you.";
                case 2:
                    return "The cat lashes their tail, but does not flee.";
                case 3:
                    return "The cat's ears turn back, eyes flicking to the side. They appear nervous, but approach you with their head down.";
            }
            return "error";
        }

        public bool Invitation(Cat catToPersuade, Cat playerCat, Clan clan, DataTable table)
        {
            Console.WriteLine("The " + catToPersuade.personality[0].ToLower() + " and " + catToPersuade.personality[1].ToLower() + " cat appears interested in what you have to say.");
            Console.WriteLine("\n[0]: Hey- Uh- Are you doing alright?\n[1]: Sit down. Let’s have a chat.\n[2]: Now, I have something to talk to you about.\n[3]: How are you? Can I get you anything?\n[4]: Nevermind, I’ve got to be going.");
            string checkIntro = "";
            while (!checkIntro.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkIntro) || int.Parse(checkIntro) > 4)
            {
                checkIntro = Console.ReadLine();

            }
            int introNumber = int.Parse(checkIntro);
            int row = -1;
            switch (introNumber)
            {
                case 0:
                    //Kind
                    row = 4;
                    break;
                case 1:
                    row = 2;
                    break;
                case 2:
                    row = 1;
                    break;
                case 3:
                    row = 6;
                    break;
                case 4:
                    return false;
            }
            float replyScore = 0;

            for (int i = 0; i < 2; i++)
            {
                replyScore += float.Parse(table.Rows[Utils.GetRow(Defs.GetPersonalityType(catToPersuade.personality[i]), table)][table.Columns[row].ColumnName].ToString());
            }
            replyScore /= 2;
            //Console.WriteLine("Reply score: " + replyScore);
            float interactionScore = GetInteractionScore(catToPersuade, playerCat, clan, table);
            //Console.WriteLine("Interaction score: " + interactionScore);
            float score = replyScore * interactionScore;
            //Console.WriteLine("Score: " + score);
            if (score >= 1)
            {
                Console.WriteLine(InvitationResponse(introNumber));
                return true;
            }
            else
            {
                Console.WriteLine("The cat looks away before shrinking off.");
                MainFile.PressEToContinue();
                return false;
            }
        }

        public string InvitationResponse(int intro)
        {
            switch (intro)
            {
                case 0:
                    return "The cat's whiskers twitch. \"I'm well, thank you.\"";
                case 1:
                    return "The cat still looks terribly nervous. \"Is this your land?\"";
                case 2:
                    return "The cat lowers their head, nodding.";
                case 3:
                    return "\"It's alright, I'm not hungry. Thank you, though.\"";
            }
            return "error";
        }

        public bool Persuade(Cat catToPersuade, Cat playerCat, Clan clan, DataTable table)
        {
            Console.WriteLine("The " + catToPersuade.personality[0].ToLower() + " and " + catToPersuade.personality[1].ToLower() + " cat leans in. Will this be a new clan member?");
            Console.WriteLine("\n[0]: Aren’t you tired of being on your own? I’ve got lots of friends!\n[1]: I am a clan cat. You may join us, but you will follow our rules and pull your own weight.\n[2]: Come with me, and you’ll never be afraid of any nasty rogues or dogs ever again.\n[3]: Me and the others would love having some extra paws around, if you’re interested.\n[4]: Nevermind, I’ve got to be going.");
            string checkIntro = "";
            while (!checkIntro.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkIntro) || int.Parse(checkIntro) > 4)
            {
                checkIntro = Console.ReadLine();

            }
            int introNumber = int.Parse(checkIntro);
            int row = -1;
            switch (introNumber)
            {
                case 0:
                    //Kind
                    row = 5;
                    break;
                case 1:
                    row = 1;
                    break;
                case 2:
                    row = 7;
                    break;
                case 3:
                    row = 6;
                    break;
                case 4:
                    return false;
            }
            float replyScore = 0;

            for (int i = 0; i < 2; i++)
            {
                replyScore += float.Parse(table.Rows[Utils.GetRow(Defs.GetPersonalityType(catToPersuade.personality[i]), table)][table.Columns[row].ColumnName].ToString());
            }
            replyScore /= 2;
            //Console.WriteLine("Reply score: " + replyScore);
            float interactionScore = GetInteractionScore(catToPersuade, playerCat, clan, table);
            //Console.WriteLine("Interaction score: " + interactionScore);
            float score = replyScore * interactionScore;
            //Console.WriteLine("Score: " + score);
            if (score >= 1)
            {
                Console.WriteLine("The cat thinks for a long while, before nodding. " + catToPersuade.name + " has joined your clan!");
                return true;
            }
            else
            {
                Console.WriteLine("\"Thanks, but no thanks. I think I'm best off alone, for now.\" With that, the strange cat walks off.");
                MainFile.PressEToContinue();
                return false;
            }
        }
    }
}

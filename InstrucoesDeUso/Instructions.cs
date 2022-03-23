using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace ProjetoPerspectivas3_0.InstrucoesDeUso
{
    public class Instructions
    {
    
        private string fileInstructions = @"Instructions.txt";

        public ListBox GetBoxInfo()
        {
            ListBox listBoxInfo = new ListBox();
      
            List<string> instructions = GetInstructionsFromFile();
            foreach (string lineOfInstruction in instructions)
                listBoxInfo.Items.Add(lineOfInstruction);

            return listBoxInfo;
        }
        public List<string> GetInstructionsFromFile()
        {
            List<string> instrucoes = new List<string>();
            StreamReader reader = new StreamReader(this.fileInstructions);
            while (!reader.EndOfStream)
            {
                string lineInstruction = reader.ReadLine();
                if (lineInstruction != null)
                    instrucoes.Add(lineInstruction);
            }
            return instrucoes;
         
        }
        
    }
}

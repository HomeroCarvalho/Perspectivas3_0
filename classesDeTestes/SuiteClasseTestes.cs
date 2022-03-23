using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using ModuloTESTES;

namespace Testes
{
    public class SuiteClasseTestes
    {
        private delegate void MetodoTeste(AssercaoSuiteClasse assercao);

        private List<MethodInfo> metodosTeste { get; set; }
        private MethodInfo metodoAntes { get; set; }
        private MethodInfo metodoDepois { get; set; }


        private string textoCaption = "";


     
        public SuiteClasseTestes(string textInfo)
        {
            this.textoCaption = textInfo;

            this.metodosTeste = new List<MethodInfo>();
           
            List<MethodInfo> metodos = this.GetType().GetMethods().ToList<MethodInfo>();

            foreach (MethodInfo umMetodoTeste in metodos) 
            {
                List<ParameterInfo> parametrosDoMetodo = umMetodoTeste.GetParameters().ToList<ParameterInfo>();

                if (umMetodoTeste.Name.Equals("Before"))
                    this.metodoAntes = umMetodoTeste;
                else
                if (umMetodoTeste.Name.Equals("After"))
                    this.metodoDepois = umMetodoTeste;
                else
                if (parametrosDoMetodo.Find(k => k.ParameterType == typeof(AssercaoSuiteClasse)) != null) 
                    this.metodosTeste.Add(umMetodoTeste);
            }

            
        }

        public void ExecutaTestes()
        {
            if (metodosTeste == null)
            {
                LoggerTests.AddMessage("Nao ha testes a serem executados nesta classe para testes.");
                return;
            }

            AssercaoSuiteClasse assercao = new AssercaoSuiteClasse();

            LoggerTests.AddMessage("Testes: " + this.textoCaption);
            foreach (MethodInfo metodo in metodosTeste)
            {
                try
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                   


                    if (metodoAntes != null)
                        metodoAntes.Invoke(this, null); // executa o metodo preparador para o teste.

                    if ((metodo.Name != "Antes") && (metodo.Name != "Depois"))
                        metodo.Invoke(this, new object[] { assercao}); // executa o teste. (nao contem parametros).

                    if (metodoDepois != null)
                        metodoDepois.Invoke(this, null); // executa o metodo finalizador para o teste.

                    stopwatch.Stop();
                    long timeElapsed = stopwatch.ElapsedMilliseconds; // calcula o tempo gasto.

                    LoggerTests.AddMessage("teste: " + metodo.Name + " executado em: " + timeElapsed.ToString() + "  milisegundos.");
              
                }
                catch (Exception exc)
                {
                    LoggerTests.AddMessage("teste: " + metodo.Name + ", na classe: " + this.GetType().Name + " gerou excecao que interrompeu o seu processamento."+" falha porque: "+exc.Message+", Stack: "+exc.StackTrace);
                    continue;
                }
            }
        }


    } // class

    public class AssercaoSuiteClasse
    {
        public bool IsTrue(bool condicaoValidacao)
        {
            if (condicaoValidacao)
            {
                LoggerTests.AddMessage("teste passou.");
                return true;
            }
            if (!condicaoValidacao)
            {
                LoggerTests.AddMessage("teste nao passou.");
                return false;
            }

            return false;
        }

    }
} // namespace

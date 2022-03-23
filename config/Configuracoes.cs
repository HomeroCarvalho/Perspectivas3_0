using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuracao
{
    public class Configuracoes
    {


        private static Configuracoes configuracaoSingleton;

        public static Configuracoes Instance()
        {
            if (configuracaoSingleton == null)
                configuracaoSingleton = new Configuracoes();
            return configuracaoSingleton;
        }


        public double anguloXZPadronizado = 11.0;
        public double anguloXYPadronizado = 00.0; // inerte para simplificação de testes.
        public double anguloYZPadronizado = 00.0; // inerte para simplificação de testes.
    }

}

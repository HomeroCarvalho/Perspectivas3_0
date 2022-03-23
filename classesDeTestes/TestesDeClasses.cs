using ProjetoPerspectivas3_0;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isometria;
using classeObjeto3dTestes;

namespace Testes
{
    public class TestesDeClasses
    {


        public void CorpoTestes()
        {

            wndMain.Testes testesJanelaIteracao = new wndMain.Testes();
            testesJanelaIteracao.ExecutaTestes();

            //Objeto3d.Testes testesObjetos3d = new Objeto3d.Testes();
            //testesObjetos3d.ExecutaTestes();


            //TransformacaoIsometricaNaoQuadratica.Testes testesTransformacaoNaoQuadratica = new TransformacaoIsometricaNaoQuadratica.Testes();
            //testesTransformacaoNaoQuadratica.ExecutaTestes();
        }
    }

}

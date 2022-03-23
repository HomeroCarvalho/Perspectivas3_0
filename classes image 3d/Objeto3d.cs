using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas;
using Testes;
using Configuracao;
using System.IO;

namespace classeObjeto3dTestes
{

   
    /// <summary>
    /// esta classe constroi objetos 3d a partir de imagens 2d com perspectiva, modifica as dimensoes do objeto 3d construido.
    /// </summary>
    public class Objeto3d
    {

        /// CLASSE PARA CONSTRUÇÃO DE IMAGENS 3D A PARTIR DE ANÁLISE DE UMA IMAGEM 2D EM PERSPECTIVA, FORMANDO UM MODELO 3D.
        /// O OBJETIVO NÃO É CRIAR UM MODELO 3D, MAS SIM UMA IMAGEM PADRONIZADA 2D E COM DIMENSÕES 3D DEFINIDAS, isometricas.
        /// ISSO PERMITE OBTER IMAGENS 2D, A PARTIR DE UM MODELO 3D EM PERSPECTIVA ISOMÉTRICA, E COM DIMENSÕES 3D PADRONIZADAS.
        /// UMA VEZ CONSEGUIDO A IMAGEM 2D PROCESSADA, PODE-SE GUARDAR EM ARQUIVO, E UTILIZAR LENDO ESTAS IMAGENS DE ARQUIVOS.


        /// estruturas de dados:
        /// pontos3d: contém todas os vetores 3d do objeto,e a cores de cada ponto.
        /// dimensoes: contém as dimensões o objeto.
        /// fileName: nome do arquivo da imagem de entrada.
        /// imageIn: imagem de entrada nao processada.
        /// imageOut: imagem de saida e processada.




        /// <summary>
        /// imagem em perspectiva de entrada, que gerará o objeto 3d. 
        /// </summary>
        public Bitmap imageIn { get; private set; }

      
        /// <summary>
        /// imagem em perspectiva tratada, do qual se estraiu pontos 3d e com estes pontos, padronizou-se a geometria do 
        /// modelo 3d gerado, com mudança de base ortonormal. 
        /// </summary>
        public Bitmap imageOut { get; private set; } 




        private List<MyVetor> pontos3d { get; set; }



        /// <summary>
        /// dimensões do objeto 3d, podendo não serem numeros inteiros, pois é uma proprorção que modifica os módulos dos vetores.
        /// </summary>
        public MyVetor dimensoes { get; set; } 




        /// <summary>
        /// arquivo da imagem de entrada, que gerará o objeto 3d.
        /// </summary>
        public string fileNameImageIn { get; set; }

        private static MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();






        private int resolucaoParaDesenho = 4;
        public int ResolutionFaces
        {
            get
            {
                return resolucaoParaDesenho;
            }
            set
            {
                if (value > 0)
                {
                    resolucaoParaDesenho = value;
                    Nivela();
		    BuildImage();
                }
            }
        }

        private byte _alphaChannelImages3d = 15;
        public byte AlphaChannel
        {
            get
            {
                return _alphaChannelImages3d;
            }
            set
            {
                if ((value >= 0) && (value <= 255) && (this.pontos3d != null) && (dimensoes != null)) 
                {
                    _alphaChannelImages3d = value;

                    // extrai vetores 3d a partir da imagem 2d em perspectiva.
                    BuildPontos3D(dimensoes);

                    // constroi o modelo 3d a partir dos vetores 3d extraidos.
                    BuildImage();
                }
            }
        }


        
        public const int vertice000 = 0;
        public const int vertice001 = 1;
        public const int vertice010 = 2;
        public const int vertice011 = 3;
        public const int vertice100 = 4;
        public const int vertice101 = 5;
        public const int vertice110 = 6;
        public const int vertice111 = 7;

     
        public Objeto3d Clone()
        {
            Objeto3d objeto = new Objeto3d(this.imageIn, this.dimensoes);
            objeto.pontos3d = this.pontos3d.ToList<MyVetor>();
            
        

            objeto.BuildImage();



            return objeto;
        }
        public Objeto3d(Bitmap _imageIn, MyVetor dimensoes)
        {

            this.dimensoes = dimensoes.Clone();



            MyVetor vt_size = MyVetor.Mul(dimensoes, transformacao.m32);
             Size size = new Size((int)vt_size.X, (int)vt_size.Y);



            this.imageIn = new Bitmap(_imageIn, size);

            this.Init(dimensoes);
        }


        public Objeto3d(string path, MyVetor dimensoes)
        {
            // calcula as dimensoes do objeto, aproximadamente.
            MyVetor dims2d = MyVetor.Mul(dimensoes, transformacao.m32);
            Size size = new Size(Math.Abs((int)dims2d.X), Math.Abs((int)dims2d.Y));



            this.fileNameImageIn = path;
            this.imageIn = new Bitmap(path);
            this.imageIn = new Bitmap(this.imageIn, size);

            Init(dimensoes);
        }

        private void Init(MyVetor dimensoes)
        {
            this.pontos3d = new List<MyVetor>();
            this.dimensoes = new MyVetor(dimensoes.X, dimensoes.Y, dimensoes.Z);


            // constroi os pontos vetoriais 3d associado à imagem.
            this.BuildPontos3D(dimensoes);

            // redimensiona os pontos 3d para as dimensões parâmetro.
            this.Resize(dimensoes);

            this.BuildImage();   
        }

        private void BuildPontos3D(MyVetor dims)
        {
            MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();

            this.pontos3d = new List<MyVetor>();
      
            for (int x = 0; x < this.imageIn.Width; x++)
                for (int y = 0; y < this.imageIn.Height; y++)
                    if (imageIn.GetPixel(x, y).A > this.AlphaChannel)
                    {
                        Color corPixel = Color.FromArgb(imageIn.GetPixel(x, y).A, imageIn.GetPixel(x, y).R, imageIn.GetPixel(x, y).G, imageIn.GetPixel(x, y).B);
                        MyVetor ponto2D = new MyVetor(corPixel,x, y);

                        MyVetor vt_point3d = transformacao.GetPonto3D(ponto2D);
                        vt_point3d.cor = Color.FromArgb(corPixel.A, corPixel.R, corPixel.G, corPixel.B);

                        pontos3d.Add(vt_point3d);
                  
                    }

        }

      


        /// <summary>
        /// constroi a image de saida, a partir de pontos 3d extraidos da analise da imagem 2d de entrada.
        /// Os pontos 3d pode ter redimensionados, rotacionados, mudados de base ortonormal, e então
        /// construida com este método, levando as cálculos mencionados para a construção da imagem.
        /// </summary>
        /// <param name="pontos3d"></param>
        private void BuildImage()
        {
            
            
            // calcula e aplica o ponto 2d minimo, para desenho da imagem de saida.
            List<MyVetor> pontos2d = new List<MyVetor>();
            for (int x = 0; x < pontos3d.Count; x++)
            {
                MyVetor pnt2d = transformacao.GetPonto2D(pontos3d[x]);
                pnt2d.cor = Color.FromArgb(pontos3d[x].cor.A, pontos3d[x].cor);
                pontos2d.Add(pnt2d);
            }


            MyVetor v_min = new MyVetor(+10000, +1000);
            MyVetor v_max = new MyVetor(-10000, -1000);

            for (int x = 0; x < pontos2d.Count; x++)
            {
                if (pontos2d[x].X < v_min.X)
                    v_min.X = pontos2d[x].X;


                if (pontos2d[x].Y < v_min.Y)
                    v_min.Y = pontos2d[x].Y;

                if (pontos2d[x].X > v_max.X)
                    v_max.X = pontos2d[x].X;

                if (pontos2d[x].Y > v_max.Y)
                    v_max.Y = pontos2d[x].Y;
            }

            for (int x = 0; x < pontos2d.Count; x++)
            {
                pontos2d[x].X -= v_min.X;
                pontos2d[x].Y -= v_min.Y;
            }



            MyVetor dims2D = MyVetor.Mul(this.dimensoes, transformacao.m32);
    
            // calculo das dimensões da imagem a ser gerada.
            int widthImage = (int)dims2D.X + 3; // um espaço de +3, devido a coordenadas nao inteiras.
            int heightImage = (int)dims2D.Y + 3;




            // constroi a imagem em que será desenhada o objeto 3d. 
            Bitmap image_current = new Bitmap(widthImage, heightImage);

         

            Graphics g = Graphics.FromImage(image_current);


            for (int x = 0; x < pontos2d.Count; x++)
            {


                // calcula o ponto de desenho na imagem de saida, a partir de pontos2d vindos da transformação isometrica.
                Point ptDRaw = new Point((int)Math.Round(pontos2d[x].X), (int)Math.Round(pontos2d[x].Y));



                // desenha um tile da imagem, a fim de não haver "furos" na imagem de saida.
                if ((ptDRaw.X >= 0) && (ptDRaw.Y >= 0) && (ptDRaw.X < image_current.Width) && (ptDRaw.Y < image_current.Height))
                    DrawPoint(ptDRaw, g, pontos2d[x].cor);
              
            } // for x


            this.imageOut = new Bitmap(image_current);
         
        }

        private void DrawPoint(Point ponto, Graphics g, Color corPonto)
        {
            SolidBrush brush = new SolidBrush(corPonto);


            Point ponto00 = new Point(ponto.X, ponto.Y);
            Point ponto01 = new Point(ponto.X, ponto.Y + ResolutionFaces);
            Point ponto11 = new Point(ponto.X + ResolutionFaces, ponto.Y + ResolutionFaces);
            Point ponto10 = new Point(ponto.X + ResolutionFaces, ponto.Y);


            Point[] pontos2d = new Point[] { ponto00, ponto01, ponto11, ponto10, ponto00 };
            g.FillPolygon(brush, pontos2d);
        } // DrawPoint()



        /// <summary>
        /// obtem as dimensões do objeto 3d.
        /// </summary>
        public MyVetor GetDimensoes()
        {

            if (pontos3d == null)
            {
                // se a imagem 2d de entrada do objeto é nula, lança uma exceção,pois não há o que fazer.
                if (this.imageIn == null)
                    throw new Exception("imagem 2d e nulo!");

                // se os vetores 3d não foram ainda calculados,  chama o método de construção de vetores 3d.
                this.BuildPontos3D(new MyVetor(1, 1, 1));
         
                // reconstroi a imagem 2d, pois os vetores 3d foi recalculado.
                this.BuildImage();

            }
            double Xmax = -10000.0;
            double Ymax = -10000.0;
            double Zmax = -10000.0;

            double Xmin = +10000.0;
            double Ymin = +10000.0;
            double Zmin = +10000.0;

            GetMin3d_Max3d(out Xmin, out Ymin, out Zmin, out Xmax, out Ymax, out Zmax);

            return new MyVetor(Xmax - Xmin, Ymax - Ymin, Zmax - Zmin);
            
        }

 
        public void Resize(MyVetor newSides)
        {

            MyVetor oldSides = this.GetDimensoes();
            MyVetor proporcao = new MyVetor(newSides.X / oldSides.X, newSides.Y / oldSides.Y, newSides.Z / oldSides.Z);
            for (int x = 0; x < pontos3d.Count; x++)
            {
                pontos3d[x].X *= proporcao.X;
                pontos3d[x].Y *= proporcao.Y;
                pontos3d[x].Z *= proporcao.Z;
            }
            this.BuildImage();
            this.dimensoes = newSides.Clone();
        }
        /// <summary>
        /// nivela o objeto 3d para obedecer um padrão de angulos, com o objetivo de criar imagens perspectiva 3d padronizadas.
        /// As imagens isometricas podem vir de varias fontes, e quase todas não seguem uma base de vetores ortonormais em comum.
        /// O nivelamento corrige as imagens isometricas, para serem utilizadas em conjunto, rotacionadas e redimensionadas sobre
        /// um padrão em comum.
        /// </summary>
            public void Nivela()
        {
            // o incremento de angulo deve setar os angulos dos vetores 3d para os angulos de configuração.

            double anguloNivelaXZ = -Configuracoes.Instance().anguloXZPadronizado;
            double anguloNivelaXY = -Configuracoes.Instance().anguloXYPadronizado;
            double anguloNivelaYZ = -Configuracoes.Instance().anguloYZPadronizado;


            // seta o vetor 3d direcional parâmetro, para rotação.
            MyVetor pontoCentral = this.PointCentral();

            MyVetor vetor_around = new MyVetor(0, 1, 0);


            // rotaciona uma base ortonormal padrão, para mudança de base para nesta base modificado.
            BaseOrtonormal ortonormal = new BaseOrtonormal();
            ortonormal.eixoX = ortonormal.eixoX.Rotate(vetor_around, anguloNivelaXY, anguloNivelaXZ, anguloNivelaYZ);
            ortonormal.eixoY = ortonormal.eixoY.Rotate(vetor_around, anguloNivelaXY, anguloNivelaXZ, anguloNivelaYZ);
            ortonormal.eixoZ = ortonormal.eixoZ.Rotate(vetor_around, anguloNivelaXY, anguloNivelaXZ, anguloNivelaYZ);


            // rotaciona todos vetores, para compatibilizar com a base ortonormal rotacionada.
            for (int x = 0; x < pontos3d.Count; x++)
            {
                // centraliza para o centro de massa, os vetores 3d.
                MyVetor.Sub(pontos3d[x], pontoCentral);



                Color corPonto = Color.FromArgb(pontos3d[x].cor.A, pontos3d[x].cor);

                pontos3d[x] = ortonormal.ParaBaseOrtogonalPadrao(pontos3d[x]); // faz o nivelamento.
                pontos3d[x].cor = Color.FromArgb(corPonto.A,corPonto);


                // descentraliza do  centro  de massa, dos vetores 3d.
                MyVetor.Add(pontos3d[x], pontoCentral);
            }
            // reconstroi a imagem 2d, pois os vetores 3d foram modificados.
            this.BuildImage(); 
        }

        public void Rotate(double  angleDegreesPlaneXY,double angleDegreesPlaneXZ, double angleDegreesPlaneYZ)
        {

            MyVetor pontoCentral = PointCentral();

            // rotaciona todos vetores, para compatibilizar com a base ortonormal rotacionada.
            for (int x = 0; x < pontos3d.Count; x++)
            {
                Color corPonto = Color.FromArgb(pontos3d[x].cor.A, pontos3d[x].cor);


                pontos3d[x] = MyVetor.Sub(pontos3d[x], pontoCentral); // centraliza os vetores 3d para o centro de massa do objeto3d.
                // >0 para rotação anti-horario, <0 para sentido horário da rotação.
                
                pontos3d[x] = pontos3d[x].Rotate(angleDegreesPlaneXZ, angleDegreesPlaneXY, angleDegreesPlaneYZ); 
                
                pontos3d[x] = MyVetor.Add(pontos3d[x], pontoCentral); // restaura os vetores 3d em relação ao ponto centro de massa.
                


                pontos3d[x].cor = Color.FromArgb(corPonto.A, corPonto);


            }

            this.BuildImage();
        }

        public void Rotate(
            MyVetor vetor_around,
            double angleDegreesPlaneXY, double angleDegreesPlaneXZ, double angleDegreesPlaneYZ)
        {




            // rotaciona uma base ortonormal padrão, para mudança de base para nesta base modificado.
            BaseOrtonormal ortonormal = new BaseOrtonormal();
            ortonormal.eixoX = ortonormal.eixoX.Rotate(vetor_around, angleDegreesPlaneXY, angleDegreesPlaneXZ, angleDegreesPlaneYZ);
            ortonormal.eixoY = ortonormal.eixoY.Rotate(vetor_around, angleDegreesPlaneXY, angleDegreesPlaneXZ, angleDegreesPlaneYZ);
            ortonormal.eixoZ = ortonormal.eixoZ.Rotate(vetor_around, angleDegreesPlaneXY, angleDegreesPlaneXZ, angleDegreesPlaneYZ);


            // rotaciona todos vetores, para compatibilizar com a base ortonormal rotacionada.
            for (int x = 0; x < pontos3d.Count; x++)
            {
                Color corPonto = Color.FromArgb(pontos3d[x].cor.A, pontos3d[x].cor);

                pontos3d[x] = ortonormal.ParaBaseOrtogonalPadrao(pontos3d[x]); // faz o nivelamento.
                pontos3d[x].cor = Color.FromArgb(corPonto.A, corPonto);
            }

            this.BuildImage();
        }

        /// <summary>
        /// desenha o box que envolve o objeto3d.
        /// </summary>
        public void DrawBox(ref Bitmap imageContainer)
        {
            if (pontos3d == null)
            {
                if (this.imageIn == null)
                    throw new Exception("imagem de entrada do objeto 3d nula.");

                this.BuildPontos3D(this.dimensoes);
                this.Resize(this.dimensoes);
                this.BuildImage();
            }

            // (0,0) porque é em relação à dentro da imagem.
            Point pnt_location = new Point(0,0); 

            // recalcula as dimensões,pois pode ser nao calculadas ainda.
            this.dimensoes = this.GetDimensoes(); 

            // transforma as dimensoes em coordenadas 2d para desenho da imagem do box 3d.
            MyVetor dims2d = MyVetor.Mul(this.dimensoes, transformacao.m32);

            double Xmin = +10000.0;
            double Ymin = +10000.0;
            double Zmin = +10000.0;

            double Xmax = -10000.0;
            double Ymax = -10000.0;
            double Zmax = -10000.0;

            GetMin3d_Max3d(out Xmin, out Ymin, out Zmin, out Xmax, out Ymax, out Zmax);
            if ((Xmin < 0) || (Ymin < 0) || (Zmin < 0))
            {
                for (int x = 0; x < pontos3d.Count; x++)
                {
                    pontos3d[x].X -= Xmin;
                    pontos3d[x].Y -= Ymin;
                    pontos3d[x].Z -= Zmin;
                }
            }


            //+5 devido a arredondamento de double para int os elemento das dimensões.
            imageContainer = new Bitmap((int)dims2d.X + 5, (int)dims2d.Y + 5); 
            Graphics g = Graphics.FromImage(imageContainer);

            Pen caneta = new Pen(Color.Red, 3.0f);

            
            List<MyVetor> vertices = new List<MyVetor>();
            for (int x = 0; x < 8; x++)
                vertices.Add(new MyVetor(0, 0, 0));

            vertices[vertice000] = new MyVetor(Xmin, Ymin, Zmin);
            vertices[vertice001] = new MyVetor(Xmin, Ymin, Zmax);

            vertices[vertice010] = new MyVetor(Xmin, Ymax, Zmin);
            vertices[vertice011] = new MyVetor(Xmin, Ymax, Zmax);

            vertices[vertice100] = new MyVetor(Xmax, Ymin, Zmin);
            vertices[vertice101] = new MyVetor(Xmax, Ymin, Zmax);

            vertices[vertice110] = new MyVetor(Xmax, Ymax, Zmin);
            vertices[vertice111] = new MyVetor(Xmax, Ymax, Zmax);


            List<Point> points = new List<Point>();
            for (int x = 0; x < 8; x++)
                points.Add(new Point());


            points[vertice000] = PointDrawBox(vertices[vertice000], pnt_location);
            points[vertice001] = PointDrawBox(vertices[vertice001], pnt_location);
            points[vertice010] = PointDrawBox(vertices[vertice010], pnt_location);
            points[vertice011] = PointDrawBox(vertices[vertice011], pnt_location);
            points[vertice100] = PointDrawBox(vertices[vertice100], pnt_location);
            points[vertice101] = PointDrawBox(vertices[vertice101], pnt_location);
            points[vertice110] = PointDrawBox(vertices[vertice110], pnt_location);
            points[vertice111] = PointDrawBox(vertices[vertice111], pnt_location);

            /// 000-->100-->101-->001-->000
            /// 010-->110-->111-->011-->010
            /// 000-->010
            /// 100-->110
            /// 101-->111
            /// 001-->011





            g.DrawLine(caneta, points[vertice000], points[vertice100]);
            g.DrawLine(caneta, points[vertice100], points[vertice101]);
            g.DrawLine(caneta, points[vertice101], points[vertice001]);
            g.DrawLine(caneta, points[vertice001], points[vertice000]);


            g.DrawLine(caneta, points[vertice010], points[vertice110]);
            g.DrawLine(caneta, points[vertice110], points[vertice111]);
            g.DrawLine(caneta, points[vertice111], points[vertice011]);
            g.DrawLine(caneta, points[vertice011], points[vertice010]);

            g.DrawLine(caneta, points[vertice000], points[vertice010]);
            g.DrawLine(caneta, points[vertice100], points[vertice110]);
            g.DrawLine(caneta, points[vertice101], points[vertice111]);
            g.DrawLine(caneta, points[vertice001], points[vertice011]);

            Font fonte = new Font("Arial", 11.5f);
            SolidBrush brush = new SolidBrush(Color.Green);


            g.DrawString("000", fonte, brush, points[vertice000]);
            g.DrawString("001", fonte, brush, points[vertice001]);
            g.DrawString("010", fonte, brush, points[vertice010]);
            g.DrawString("011", fonte, brush, points[vertice011]);
            g.DrawString("100", fonte, brush, points[vertice100]);
            g.DrawString("101", fonte, brush, points[vertice101]);
            g.DrawString("110", fonte, brush, points[vertice110]);
            g.DrawString("111", fonte, brush, points[vertice111]);



        }

        private void GetMin3d_Max3d(out double Xmin, out double Ymin, out double Zmin, out double Xmax, out double Ymax, out double Zmax)
        {
            Xmin = +1000;
            Ymin = +1000;
            Zmin = +1000;
            Xmax = -1000;
            Ymax = -1000;
            Zmax = -1000;
            for (int x = 0; x < pontos3d.Count; x++)
            {
                if (pontos3d[x].X < Xmin)
                    Xmin = pontos3d[x].X;

                if (pontos3d[x].Y < Ymin)
                    Ymin = pontos3d[x].Y;

                if (pontos3d[x].Z < Zmin)
                    Zmin = pontos3d[x].Z;

                if (pontos3d[x].X > Xmax)
                    Xmax = pontos3d[x].X;

                if (pontos3d[x].Y > Ymax)
                    Ymax = pontos3d[x].Y;

                if (pontos3d[x].Z > Zmax)
                    Zmax = pontos3d[x].Z;


            }
        }

     

        private Point PointDrawBox(MyVetor v3d, Point location)
        {
            MyVetor vt_point = MyVetor.Mul(v3d, transformacao.m32);

            return new Point((int)vt_point.X + location.X,
                             (int)vt_point.Y + location.Y);
        }

        private MyVetor PointCentral()
        {
            MyVetor center = new MyVetor(0, 0, 0);
            for (int x = 0; x < pontos3d.Count; x++)
            {
                center.X += pontos3d[x].X;
                center.Y += pontos3d[x].Y;
                center.Z += pontos3d[x].Z;
            }
            center.X /= (double)pontos3d.Count;
            center.Y /= (double)pontos3d.Count;
            center.Z /= (double)pontos3d.Count;

            return center;
        }

        public class Testes : SuiteClasseTestes
        {
            Objeto3d umEdificio;
            Objeto3d umEdificio2;
            MyVetor dimensoes = new MyVetor(20, 20, 20);
            MyVetor location = new MyVetor(100, 100, 0);
            Bitmap image_box = null;
            Bitmap image_ref = null;
            string captionTest = null;

            public Testes():base("teste para objetos 3d.")
            {
                
            }

            public void Teste_DesenhaBox3D(AssercaoSuiteClasse asssercao)
            {
                this.captionTest = "Desenho Box 3d.";

                string fileNameImage = @"..\..\images\boxIsometric.png";
                umEdificio = new Objeto3d(fileNameImage, new MyVetor(80, 80, 80));
                image_ref = new Bitmap(fileNameImage);
                image_ref = new Bitmap(image_ref, new Size(120, 120));

                Form wnd_teste_box = new Form()
                {
                    Location = new Point(0, 0),
                    Size = new Size(800, 600)
                };
                umEdificio.DrawBox(ref this.image_box);
                wnd_teste_box.Paint += Wnd_teste_box_Paint;
                wnd_teste_box.ShowDialog();
            }

            public void Teste_DrawObject3D(AssercaoSuiteClasse assercao)
            {

                this.captionTest = "Teste desenho objeto 3d.";

                string fileNameImage = @"..\..\images\prefeitura.png";

                umEdificio = new Objeto3d(fileNameImage, new MyVetor(70, 70, 70));
                umEdificio.resolucaoParaDesenho = 3;

                this.captionTest = "Desenho Objeto 3d";

                this.dimensoes = umEdificio.GetDimensoes();

                assercao.IsTrue(true); // desenho da imagem 2d construída sem erros fatais.
                assercao.IsTrue(Math.Abs(umEdificio.GetDimensoes().X - 70) < 0.1); // dimensões do objeto 3d validadas.
                assercao.IsTrue(umEdificio.imageOut != null); // imagem do objeto 3d construída.
                DrawInWindow();

            }



     


            private void Wnd_teste_box_Paint(object sender, PaintEventArgs e)
            {
                if (umEdificio.imageOut != null)
                {
                    e.Graphics.DrawString(this.captionTest, new Font("Arial", 11.0f), new SolidBrush(Color.Red), 0, 0);

                    MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();
                    MyVetor vt_position = MyVetor.Mul(this.location, transformacao.m32);
                    Point posicao = new Point((int)vt_position.X, (int)vt_position.Y);

                    e.Graphics.DrawImage(this.umEdificio.imageOut, posicao);
                    if (this.image_box != null)
                        e.Graphics.DrawImage(this.image_box, posicao);

                    if (this.image_ref != null)
                        e.Graphics.DrawImage(this.image_ref, new Point(500, 100));
                }
            }


            public void Teste_NivelaObjeto3d(AssercaoSuiteClasse assercao)
            {
                string fileNameImage = @"..\..\images\boxIsometric.png";
                this.captionTest = "Teste Nivelamento objeto 3d.";

                umEdificio = new Objeto3d(fileNameImage, new MyVetor(70, 70, 70));
                int indexAleatorioVetorValidacao = new Random(1503).Next(umEdificio.pontos3d.Count);

                umEdificio.resolucaoParaDesenho = 3;
                MyVetor vetorAntesNivelamento = umEdificio.pontos3d[indexAleatorioVetorValidacao].Clone();
                umEdificio.Nivela();
                MyVetor vetorAposNivelamento = umEdificio.pontos3d[indexAleatorioVetorValidacao].Clone();

                umEdificio2 = new Objeto3d(fileNameImage, new MyVetor(70, 70, 70));
                umEdificio.resolucaoParaDesenho = 3;


                this.dimensoes = umEdificio.GetDimensoes();

                assercao.IsTrue(Math.Abs(
                    Math.Abs(vetorAposNivelamento.X) -
                    Math.Abs(vetorAntesNivelamento.X * ToRadianos(Configuracoes.Instance().anguloXZPadronizado))) < 0.1);

                this.DrawInWindow();

            }


            public void Teste_ResizeObjeto3D(AssercaoSuiteClasse assercao)
            {
                this.captionTest = "Teste Redimensionamento de objeto 3d.";

                string fileNameImage = @"..\..\images\boxIsometric.png";

                umEdificio = new Objeto3d(fileNameImage, new MyVetor(70, 70, 70));
                umEdificio.resolucaoParaDesenho = 3;
                umEdificio.Resize(new MyVetor(100, 100, 100));

                this.dimensoes = umEdificio.GetDimensoes();

                // valida as novas dimensões do objeto 3d.
                assercao.IsTrue((Math.Abs(100 - dimensoes.X) < 0.1) &&
                                (Math.Abs(100 - dimensoes.Y) < 0.1) &&
                                (Math.Abs(100 - dimensoes.Z) < 0.1));

                this.DrawInWindow();
            }

            public void Teste_RotacionaObjeto3D(AssercaoSuiteClasse assercao)
            {

                this.captionTest = "Teste Rotaciona objeto 3d.";

                string fileNameImage = @"..\..\images\boxIsometric.png";


                umEdificio = new Objeto3d(fileNameImage, new MyVetor(70, 70, 70));
                umEdificio.resolucaoParaDesenho = 3;
                MyVetor dimensoesAntesRotacao = umEdificio.GetDimensoes();


                umEdificio.Rotate(0.0, -15.0, 0.0);


                umEdificio2 = new Objeto3d(fileNameImage, new MyVetor(70, 70, 70));
                umEdificio2.resolucaoParaDesenho = 3;

                this.dimensoes = umEdificio.GetDimensoes();

                assercao.IsTrue(Math.Abs(this.dimensoes.X - dimensoes.X * ToRadianos(-15)) < 0.1);

                this.DrawInWindow();

            }
            private void DrawInWindow()
            {
                Form wndDrawBitmapObjeto3d = new Form()
                {
                    Location = new Point(50, 50),
                    Size = new Size(800, 600)
                };

                wndDrawBitmapObjeto3d.Paint += WndTest1_Paint;
                wndDrawBitmapObjeto3d.ShowDialog();
            }

            private void WndTest1_Paint(object sender, PaintEventArgs e)
            {
                if (umEdificio != null)
                {

                    if (this.umEdificio != null)
                    {
                        e.Graphics.DrawString(this.captionTest, new Font("Arial", 11.0f), new SolidBrush(Color.Red), 0, 0);

                        e.Graphics.DrawImage(this.umEdificio.imageOut, new Point(50, 50));
                        e.Graphics.DrawString(
                            "dimensoes do edificio: " +
                            dimensoes.X.ToString("N1") + "," +
                            dimensoes.Y.ToString("N1") + "," +
                            dimensoes.Z.ToString("N1"),
                            new Font("Arial", 12.0f), new SolidBrush(Color.DarkGreen), new PointF(0, 300));
                    }
                    if (this.umEdificio2 != null)
                        e.Graphics.DrawImage(umEdificio2.imageOut, new Point(50, 200));
                }
            }

  

            private double ToRadianos(double anguloEmGraus)
            {
                return (anguloEmGraus * Math.PI / 180.0);
            }


            public void Teste_InstantiateObject3d(AssercaoSuiteClasse assercao)
            {
                this.captionTest = "Teste Instanciacao de objeto 3d.";
                string fileNameImage = @"..\..\images\prefeitura.png";

                umEdificio = new Objeto3d(fileNameImage, new MyVetor(40, 40, 40));

                assercao.IsTrue(umEdificio!=null); // o objeto 3d foi instanciado sem erros fatais.

            }

          

            public void Teste_ObtemImageIntoProject(AssercaoSuiteClasse assercao)
            {

                this.captionTest = "Teste get image into project.";
                string fileNameImageTest = Path.GetFullPath(@"..\..\images\prefeitura.png");

                Bitmap imageTestFileName = new Bitmap(fileNameImageTest);

                assercao.IsTrue(true); // o teste para obter a imagem dentro do projeto ocorreu como esperado.

            }

        }


    } // class Objeto3d



} // namespace

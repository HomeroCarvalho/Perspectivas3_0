using ModuloTESTES;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MatrizesNaoQuadraticas;
using Classes3D.Util;
using Testes;
using classeObjeto3dTestes;
using ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas;

namespace ProjetoPerspectivas3_0
{
    public partial class wndMain : Form
    {

        public Size size_images2d = new Size(75, 75);

        public MyVetor dimensoes_image3d = new MyVetor(50, 50, 50);

        public Bitmap imageIn;
        public Bitmap imageOut;

        private string planeRotation = "";
        private double angleRotate = 5.0;

        private Objeto3d objetoCurrente;

        private int qtdImagesRotated = 8;
        public wndMain()
        {
            InitializeComponent();
            this.Size = new Size(1000, 700);
            this.Text = "Janela de Rotacoes";

         
        }

        public void BuildImageOut(Bitmap imageIn)
        {
            this.objetoCurrente = new Objeto3d(imageIn, dimensoes_image3d);

            this.imageOut = this.objetoCurrente.imageOut;

            int wdth = (int)(3.25 * imageOut.Width);
            int hght = (int)(3.25 * imageOut.Height);

            this.pctBxImageOut.Image = new Bitmap(wdth, hght);
            this.pctBxImageOut.Size = new Size(wdth, hght);

            this.pctBxImageOut.Image = this.imageOut;

            this.Refresh();
        }


        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PNG IMAGE FILES|*.png";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                
                // limpa as areas onde estavam as imagens do processamento anterior.
                pctBxImageIn.Image = new Bitmap(size_images2d.Width, Size.Height);
                pctBxImageOut.Image = new Bitmap(size_images2d.Width, Size.Height);



                // faz a leitura do arquivo da imagem de entrada.
                this.imageIn = new Bitmap(dialog.FileName);
                this.imageIn = new Bitmap(this.imageIn, size_images2d);



         

                // mostra a imagem de entrada, num picture box.
                this.pctBxImageIn.Image = (Image)this.imageIn;


                this.Refresh();

                // constroi a imagem processado.
                this.BuildImageOut(this.imageIn);
            }


        }

        private void saveImageProcessedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG IMAGE FILES|*.png";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.imageOut.Save(dialog.FileName);
                MessageBox.Show("Image Processed save.");
            }
        }

        private void btnCreateRotateImages_Click(object sender, EventArgs e)
        {
            if (planeRotation == "")
            {
                MessageBox.Show("Choice a rotate plane, in combo box for rotates planes.");
                return;
            }
            else
                this.BuildImagesRotated(this.objetoCurrente, this.planeRotation);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexPlane = this.cmbBxPlaneRotation.SelectedIndex;
            if (indexPlane != -1)
                switch (indexPlane)
                {
                    case 0: 
                        planeRotation = "XZ";
                        break;

                    case 1:
                        planeRotation = "XY";
                        break;

                    case 2:
                        planeRotation = "YZ";
                        break;

                }

        }

        

        private void BuildImagesRotated(Objeto3d objetoRaiz, string planeRotation)
        {
         
            // cria uma nova imagem no picture box para imagens rotacionadas.
            pctBxImagesRotated.Image = new Bitmap(pctBxImagesRotated.Width, pctBxImagesRotated.Height);


          
            double planeXZ = 0.0;
            double planeXY = 0.0;
            double planeYZ = 0.0;

            switch (planeRotation)
            {
                case "XZ":
                    planeXZ = angleRotate;
                    break;
                case "YZ":
                    planeYZ = angleRotate;
                    break;
                case "XY":
                    planeXY = angleRotate;
                    break;
            }

            Graphics g = Graphics.FromImage(pctBxImagesRotated.Image);
            List<Bitmap> imagensRotacionadas = new List<Bitmap>();
          
            int offsetWidth = 0;

            MyVetor vt_emVolta = new MyVetor(0.0, 1.0, 0.0);
            for (int x = 0; x < qtdImagesRotated; x++)
            {
                // cria um objeto clonado, a partir do objeto original sem rotações.
                Objeto3d objetoCurrente = objetoRaiz.Clone();

                // rotaciona o objeto currente.
                objetoCurrente.Rotate(vt_emVolta, planeXZ, planeXY, planeYZ);


                imagensRotacionadas.Add(new Bitmap(objetoCurrente.imageOut));


                // desenha a imagem rotacionada, na imagem do picture box de imagens rotacionadas.
                g.DrawImage(imagensRotacionadas[imagensRotacionadas.Count - 1], new PointF(offsetWidth, 0));


                // incrementa a posição de desenho, no picture box de imagens rotacionadas.
                offsetWidth += imagensRotacionadas[imagensRotacionadas.Count - 1].Width + 10;

                if (planeXZ != 0.0)
                    planeXZ += angleRotate;

                if (planeYZ != 0.0)
                    planeYZ += angleRotate;

                if (planeXY != 0.0)
                    planeXY += angleRotate;
              
            }
            this.Refresh();
        }

        public class Animate3d
        {
            private List<Objeto3d> lstObjetosRotacionados { get; set; }


            private int indexAnimtation { get; set; }

            private int qtdAnimations { get; set; }

            private TimeReaction temporizadorDeAnimacao;
            private TimeReaction tempoDeAnimacao;
            public Animate3d(Objeto3d objetoOriginal, int qtdAnimations)
            {
                // define o angulo plano xz para rotação, e o incremento de ângulo, a cada mudança de frame.
                double stepAngle = 20.0 / (double)qtdAnimations;
                double angleRotateXZ = 0.0;
                
                
                this.lstObjetosRotacionados = new List<Objeto3d>();


                for (int x = 0; x < qtdAnimations; x++)
                {
                    Objeto3d objRotated = objetoOriginal.Clone();
                    objRotated.Rotate(angleRotateXZ, 0.0, 0.0);
                    this.lstObjetosRotacionados.Add(objRotated);

                    angleRotateXZ += stepAngle;
                }



                this.indexAnimtation = 0;
                this.qtdAnimations = qtdAnimations;

                this.temporizadorDeAnimacao = new TimeReaction(1000.0 / 20.0); // 20 fps para trocar de frame.
                this.tempoDeAnimacao = new TimeReaction(5 * 1000.0); // 5 segundos de animação.
            }

            public void Run(PictureBox wndViewAnimation)
            {
                wndViewAnimation.Image = new Bitmap(wndViewAnimation.Width, wndViewAnimation.Height);
                
                int signalAnimation = +1;
                Graphics gimage = Graphics.FromImage(wndViewAnimation.Image);

                // enquanto nao passa o tempo total da animação, atualiza e desenha os frames.
                while (!this.tempoDeAnimacao.IsTimeToAct())
                {
                    // faz o processamento de qualquer evento que aconteça, impedindo do laço trave o fluxo da aplicação.
                    Application.DoEvents();

                    // se o tempo de pausa para visualização de um frame passou, desenha outro frame.
                    if (this.temporizadorDeAnimacao.IsTimeToAct())
                    {
                        // atualiza o indice de frame.
                        this.indexAnimtation += signalAnimation;

                        // se o indice de frame ultrapassar a quantidade de animação, inverte o incremento do indice de frame.
                        if (this.indexAnimtation >= this.qtdAnimations)
                        {
                            signalAnimation = -1;
                            this.indexAnimtation--;
                        }
                        else
                        // se o indice de frame com o incremento, for menor que 0, inverte o incremento do indice de frame.
                        if (this.indexAnimtation < 0)
                        {
                            signalAnimation = +1;
                            this.indexAnimtation++;
                        }

                        // preenche com cor, a janela de desenho.
                        gimage.Clear(Color.Black);
                        // desenha o frame.
                        gimage.DrawImage(this.lstObjetosRotacionados[this.indexAnimtation].imageOut, new Point(0, 0));

                        wndViewAnimation.Refresh();

                    }
                }
                gimage.Clear(Color.Black);
                
            }
        }

        public class Testes: SuiteClasseTestes
        {
            public Testes():base("Testes da janela de iteracao")
            {

            }
            public void Teste_01(AssercaoSuiteClasse assercao)
            {
               
                wndMain wndIteracao = new wndMain()
                {
                    Size = new Size(800, 600),
                    Location = new Point(50, 50)

                };

                wndIteracao.ShowDialog();
            }
        }

        private void btnCreateAnimation_Click(object sender, EventArgs e)
        {
            // cria uma pequena animação, rotacionando o objeto no planho XZ, a cada criacao do frame,
            Animate3d animacao = new Animate3d(this.objetoCurrente, 20);

            // roda a animação.
            animacao.Run(this.pctBxAnimationObjectRotate);
        }

        private void txtBxAngleRotation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    this.angleRotate = double.Parse(txtBxAngleRotation.Text.ToString());
                    this.BuildImagesRotated(this.objetoCurrente, this.planeRotation);
                }
                catch
                {
                    MessageBox.Show("Enter with a valid double number!");
                }
            }
        }

        private void wndMain_Load(object sender, EventArgs e)
        {
            InstrucoesDeUso.Instructions instrucoes = new InstrucoesDeUso.Instructions();
            this.lstBxInstructions.Items.AddRange(instrucoes.GetBoxInfo().Items);
        }


    }
}

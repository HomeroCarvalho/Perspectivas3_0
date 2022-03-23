using myControlsLibrary;
using myControlsLibrary.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MATRIZES;
using Utils;
using SdlDotNet.Input;
using MyControlsForGames_3.renderizacao;
namespace ModuloTESTES
{
    public class CorpoTestes
    {
       
        
        private BoxInfo umBox = null;
        public void TesteBoxInfo(Assercoes assercao)
        {


            int id_window = Render.Instance().AddWindow(new Size(800, 600));
            // inicializa um box de informações.
            this.umBox = new myControlsLibrary.BoxInfo("Este é o teste para Box info", new vetor2(50, 50));
           
            LoopGame.Instance().Run();
        } // TesteBoxInfo()

        BarraDeMensagens barra = null;
        Form wndBarraDeMensagens;
        private void TesteBarraDeMensagens(Assercoes assercao)
        {
            this.wndBarraDeMensagens = new Form()
            {
                Location = new Point(0, 0),
                Size = new Size(1020, 600)
            };

           int id_window= Render.Instance().AddWindow(wndBarraDeMensagens.Size);

            // constroi uma barra de mensagens.
            this.barra = new BarraDeMensagensHorizontal(Color.Yellow, wndBarraDeMensagens, new SdlDotNet.Graphics.Font("ALGER.TTF", 15));

            // adciona a mensagen à barra.
            this.barra.AddMessage("As ações da Eletrobrás subiram +10%", "icons\\sueiIcon.png", Color.Yellow);
            this.barra.AddMessage("As ações do Banco do Brasil subiram +1%", "icons\\aprovacaoIcon.png", Color.Yellow);
            this.barra.AddMessage("As ações da Vale cairam 0.1%","icons\\happyIcon.png", Color.Yellow);

            assercao.MsgSucess("construção de uma barra de rolagem feito sem erros fatais.");


            // mostra o formulário.                
            wndBarraDeMensagens.Show();

            
            LoopGame.Instance().RegistryObject(this.barra.Update, this.barra.Draw);
            LoopGame.Instance().Run(); 

            
        } // TesteBarraDeMensagens()



        ControlTime controlTime = null;
        private void TesteControlTime(Assercoes assercao)
        {
            
            Form wndCntrlTime = new Form()
            {
                Location = new Point(0, 0),
                Size = new Size(800, 600)
            };

            int id_window= Render.Instance().AddWindow(wndCntrlTime.Size);
            // inicializa um objeto control time, para o teste.
            this.controlTime = new ControlTime(Color.Yellow, wndCntrlTime, new vetor2(10, 10), id_window);

            // seta uma contagem até zero.
            this.controlTime.SetTimeDecrescent(5, 5);

            // registra um objeto para aprarecer no loop.
            LoopGame.Instance().RegistryObject(controlTime.Update, controlTime.Draw);

            // mostra o form container.
            wndCntrlTime.Show();

            // chama o loop do game.
            LoopGame.Instance().Run();

        } // TesteControlTime()

        Form wndChangerColors = null;
        Bitmap imageIn = null;
        Bitmap imageOut = null;
        private void TesteChangerColors(Assercoes assercao)
        {

            this.wndChangerColors = new Form()
            {
                Location = new Point(0, 0),
                Size = new Size(800, 600)
            };

            this.wndChangerColors.Paint += WndChangerColors_Paint;

            this.imageIn = new Bitmap("jungle1.jpg");
            this.imageIn = new Bitmap(this.imageIn, 150, 150);

            List<int> qtdDeCadaCor = new List<int>();
            ChangerColors changer = new ChangerColors();

            List<Cluster> clusters = Cluster.GetClusters(this.imageIn);
            if ((clusters != null) && (clusters.Count > 0)) 
            {
                this.imageOut = changer.ChangeOneColor(Color.White, this.imageIn, clusters[0]); // tolerância de 30%.


                assercao.MsgSucess("modificador de cores inicializado e método de mudança de uma cor, feito sem erros fatais.");
            }
            this.wndChangerColors.ShowDialog();

        } // TesteChangerColors()

        private void WndChangerColors_Paint(object sender, PaintEventArgs e)
        {
            if (this.imageOut != null)
            {
                e.Graphics.DrawImage(this.imageOut, new PointF(0.0f, 0.0f));
            } // if
            if (this.imageIn != null)
            {
                e.Graphics.DrawImage(this.imageIn, new PointF(200.0f, 0.0f));
            }
        } // WndChangerColors_Paint()



        private void TesteChangerColors_2(Assercoes assercao)
        {

            this.wndChangerColors = new Form()
            {
                Location = new Point(0, 0),
                Size = new Size(800, 600)
            };

            this.wndChangerColors.Paint += WndChangerColors_Paint;

            this.imageIn = new Bitmap("jungle1.jpg");
            this.imageIn = new Bitmap(this.imageIn, 170, 170);

            // forma uma lista de cores que modificará as matizes de cores de vários clusters de cores da imagem de entrada.
            List<Color> coresSubstitutas = new List<Color>() { Color.White, Color.Red, Color.Yellow, Color.Blue };

            ChangerColors changer = new ChangerColors();
            this.imageOut = changer.ChangeManyColors(coresSubstitutas, this.imageIn, 30.0); // tolerância de 30%.

            assercao.MsgSucess("modificador de cores inicializado, feito sem erros fatais.");

            this.wndChangerColors.ShowDialog();

        } // TesteChangerColors_2()

        
        private void TesteBarra2Estagios(Assercoes assercao)
        {
            Form wndTesteBarra = new Form()
            {
                Location = new Point(0, 0),
                Size = new Size(1000, 800)
            };

            int id_window= Render.Instance().AddWindow(wndTesteBarra.Size);

            BarraDeMensagensDoisEstagios barra = new BarraDeMensagensDoisEstagios(Color.Blue, wndTesteBarra);


            barra.AddMessage("As ações da Eletrobrás subiram +10%", "icons\\angryIcon.png", Color.Yellow);
            barra.AddMessage("As ações do Banco do Brasil subiram +15%", "icons\\diabloIcon.png", Color.Yellow);
            barra.AddMessage("As ações da Vale cairam 0.1%", "icons\\happyIcon.png", Color.Yellow);


            LoopGame.Instance().SizeScreen = wndTesteBarra.ClientRectangle.Size;
            LoopGame.Instance().RegistryObject(barra.Update, barra.Draw);

            wndTesteBarra.Show();
            LoopGame.Instance().Run();
           
        } // TesteBarra2Estagios()

        private void TesteIconAvatar(Assercoes assercao)
        {
            Form wndTestsIconAvatar = new Form()
            {
                Location = new Point(100, 0),
                Size = new Size(800, 600)
            };

            int id_render = Render.Instance().AddWindow(wndTestsIconAvatar.Size);
            ProductionIcon avatar = new ProductionIcon(new Bitmap("icons\\happyIcon.png"), new vetor2(100, 200)); // cria um icon avatar.
            ProductionIcon avatarTexto = new ProductionIcon("+1 food", 15.0f, new vetor2(100, 250));

            LoopGame.Instance().SizeScreen = wndTestsIconAvatar.Size;
            LoopGame.Instance().RegistryObject(avatar.Update, avatar.Draw); // registra o icon avatar no loop game.
            LoopGame.Instance().RegistryObject(avatarTexto.Update, avatarTexto.Draw);

            wndTestsIconAvatar.Show(); // mostra o form.

            LoopGame.Instance().Run(); // inicia o loop.

            assercao.MsgSucess("construção de icon avatar feito sem erros fatais.");
        } // TesteIconAvatar()

        private void testeHudRadar(Assercoes assercao)
        {



            int id_window = Render.Instance().AddWindow(new Size(800, 600));

            Bitmap imageBackGround = new Bitmap("backgrounds\\background1.png");
            imageBackGround = new Bitmap(imageBackGround, 2000, 2000);



            HudRadar radar = new HudRadar(new vetor2(0, 0), imageBackGround, 0.1f, id_window); // constroi um objeto space radar.

            ObjectMapHuman myShip = new ObjectMapHuman(new Bitmap("StarShipMain.png"), new vetor2(500.0,500.0), 5.0, id_window); // controi uma nave para o player humano.
            radar.SetShipPlayer(myShip); // seta o objeto mapa do jogador humano.



            // inicializa algums objetos a serem mostrado em escala.
            radar.Add(new Bitmap("StarShip1.png"), ObjectLocationTest3, new vetor2(400, 500));
            radar.Add(Color.Blue, ObjectLocationTest1, new vetor2(900, 500));
            radar.Add(Color.Yellow, ObjectLocationTest2, new vetor2(180, 100));

          
            LoopGame.Instance().RegistryObject(radar.Update, radar.Draw);
            LoopGame.Instance().Run();
           
        } // TesteSpaceRadar()

        private vetor2 locationUpdateShipMain()
        {
            return new vetor2(0, 0);
        }
        private void TesteMapaEmScala(Assercoes assercao)
        {
            this.imagemSpaceShip = new Bitmap("StarShipMain.png");
            
            // constroi o mapa em escala real, a partir de uma imagem de fundo.
            Bitmap imageBackGround = new Bitmap("backgrounds\\background1.png");
            
            
            imageBackGround = new Bitmap(imageBackGround, 2000, 2000);




            int id_window = Render.Instance().AddWindow(new Size(1000, 700));
            MapInScale map = new MapInScale(new vetor2(0, 100), imageBackGround, 0.14F, id_window);


            // inicializa objetos a serem mostrado em escala.
            map.Add(new Bitmap("StarShip1.png"), ObjectLocationTest3, new vetor2(600, 600));
            map.Add(Color.Blue, ObjectLocationTest1, new vetor2(400, 100));
            map.Add(Color.Yellow, ObjectLocationTest2, new vetor2(500, 500));

            LoopGame.Instance().SizeScreen = new Size(1000, 700);
            LoopGame.Instance().RegistryObject(map.Update, map.Draw);
            LoopGame.Instance().Run();

        } // TesteMapScale()

       
     
        Bitmap imagemSpaceShip = null;
        vetor2 locationTest1 = new vetor2(100, 5);
        vetor2 locationTest2 = new vetor2(150, 100);
        vetor2 locationTest3 = new vetor2(200, 50);

        private vetor2 ObjectLocationTest0()
        {
            return new vetor2(+5, +0);
        } // GetLocationTest1()


        private vetor2 ObjectLocationTest1()
        {
            return new vetor2(+0,+10);
        } // GetLocationTest1()

        private vetor2 ObjectLocationTest2()
        {
            return new vetor2(+5, +5);
        } // GetLocationTest2()
        private vetor2 ObjectLocationTest3()
        {
            return new vetor2(+5, -5);
        } // GetLocationTest2()


        private void TesteJoystick(Assercoes assercao)
        {
            
            JoystickVetorial joystick = new JoystickVetorial(new Point(0, 400), 100.0);
            
            LoopGame.Instance().Run();

        } // TesteJoystick()

 
        private void TesteObtemAngulo(Assercoes assercao)
        {
            vetor2 vetorTeste1 = new vetor2(1.0, 0.0);

            double anguloQuadrante1 = vetorTeste1.GetAngle();
            if (anguloQuadrante1 == 0.0)
                assercao.MsgSucess("Teste de obtenção de ângulo 1 feita com exatidão,");

            vetor2 vetorTeste2 = new vetor2(1.0, 1.0);
            double anguloQuadrante2 = vetorTeste2.GetAngle();
            if (anguloQuadrante2 == 45.0)
                assercao.MsgSucess("Teste de obtenção de ângulo 2 feita com exatidão.");

            vetor2 vetorTeste3 = new vetor2(-1.0, 0.0);
            double anguloQuadrante3 = vetorTeste3.GetAngle();
            if (anguloQuadrante3 == 180.0)
                assercao.MsgSucess("Teste de obtenção de ângulo 3 feita com exatidão.");
        } // TesteObtemAngulo()

    
        ControlCustomColors controlCustomizado = null;
        private void TesteControlCustomColors(Assercoes assercao)
        {
            Form wndControlColors = new Form()
            {
                Location = new Point(0, 0),
                Size = new Size(800, 600)
            };
            wndControlColors.Paint += WndControlColors_Paint;
            int id_window = Render.Instance().AddWindow(wndControlColors.Size);

            ComboBox textBoxTeste = new ComboBox();
            textBoxTeste.Size = new Size(150, 50);
            textBoxTeste.Location = new Point(50, 0);
            textBoxTeste.Text = "+1 food";

            List<Color> lstColorsSubstitutas = new List<Color>(); // inicia uma lista de cores substitutas.
            lstColorsSubstitutas.Add(Color.Blue);
            lstColorsSubstitutas.Add(Color.Yellow);
            lstColorsSubstitutas.Add(Color.Yellow);

            Bitmap imagemEntrada = null;
            textBoxTeste.DrawToBitmap(imagemEntrada, textBoxTeste.ClientRectangle);



            this.controlCustomizado = new ControlCustomColors(lstColorsSubstitutas, imagemEntrada, wndControlColors);
    
            
            wndControlColors.ShowDialog();
            
        } // TesteControlCustomColors()

        private void WndControlColors_Paint(object sender, PaintEventArgs e)
        {
            if (controlCustomizado.imageOut != null)
                e.Graphics.DrawImage(controlCustomizado.imageOut, new PointF(0, 100));

        } // WndControlColors_Paint()


       
        Form wndTestesStateMachine = null;
        ComboBox cmbTestesStateMachine = null;
        
        private void TesteStateMachine(Assercoes assercao)
        {
            this.wndTestesStateMachine = new Form()
            {
                Location = new Point(0, 0),
                Size = new Size(800, 600)
            };
            this.cmbTestesStateMachine = new ComboBox()
            {
                Location = new Point(100, 100),
                Size = new Size(350, 30)
            };

            this.wndTestesStateMachine.Controls.Add(this.cmbTestesStateMachine);

            int id_window = Render.Instance().AddWindow(wndTestesStateMachine.Size);


            StateMachine maquinaDeEstados = new StateMachine(); // inicializa uma máquina de estados.
            maquinaDeEstados.AddState("move", StateMove); // estado move.
            maquinaDeEstados.AddState("idle", StateIdle); // estado idle.
            maquinaDeEstados.AddState("atack", StateAtack); // estado atack.

            maquinaDeEstados.SetState("idle");

            wndTestesStateMachine.Show();
            LoopGame.Instance().SizeScreen = wndTestesStateMachine.Size;
            LoopGame.Instance().AddUpdateMethod(maquinaDeEstados.Update);
            LoopGame.Instance().Run();


            assercao.MsgSucess("Teste para StateMachine feito sem erros fatais.");
           
        } // TesteStateMachine()

        private bool StateMove()
        {
            this.cmbTestesStateMachine.Items.Add("State Move is Active");
            return true;
        }

        private bool StateIdle()
        {
            this.cmbTestesStateMachine.Items.Add("State Idle is Active.");
            return true;
        }

        private bool StateAtack()
        {
            this.cmbTestesStateMachine.Items.Add("State Atack is Active");
            return true;
        }

        TextBox txtBxOnMouseTeste;
        TextBox txtBxOnMousePosition;
        TextBox txtBxOnMouseAreaInfo;
        Point locationArea;
        Size szAreaAtiva;
        TimeReaction tempoDeAtualizacaoMouseState;
        Form wndTesteOnMouseMove;
        Bitmap imgAreaAtiva;


        private void TesteOnMouse(Assercoes assercao)
        {
            this.locationArea = new Point(50, 50);
            this.szAreaAtiva = new Size(100, 100);
            this.imgAreaAtiva = new Bitmap(this.szAreaAtiva.Width, this.szAreaAtiva.Height);

            Graphics g = Graphics.FromImage(imgAreaAtiva);
            g.Clear(Color.Yellow);

            
            this.tempoDeAtualizacaoMouseState = new TimeReaction(1000.0 / 20.0);
            

             this.wndTesteOnMouseMove= new Form()
            {
                Location = new Point(100,100),
                Size = new Size(800, 600)
            };

            Rectangle rectUmaAreaAtiva = new Rectangle(this.locationArea.X, this.locationArea.Y, szAreaAtiva.Width, szAreaAtiva.Height);

            this.wndTesteOnMouseMove.Paint += WndTesteOnMouseMove_Paint;
            txtBxOnMouseTeste = new TextBox()
            {
                Location = new Point(0, 400),
                Size = new Size(500, 35)
            };

            txtBxOnMousePosition = new TextBox()
            {
                Location = new Point(0, 450),
                Size = new Size(500, 35)
            };

            txtBxOnMouseAreaInfo = new TextBox()
            {
                Location= new Point(0,500),
                Size= new Size(500,35)
            };

            txtBxOnMouseAreaInfo.Text = "X: " + rectUmaAreaAtiva.X + " , Y: " + rectUmaAreaAtiva.Y + " width: " + rectUmaAreaAtiva.Width + "  height: " + rectUmaAreaAtiva.Height;

            wndTesteOnMouseMove.Controls.Add(txtBxOnMouseTeste);
            wndTesteOnMouseMove.Controls.Add(txtBxOnMousePosition);
            wndTesteOnMouseMove.Controls.Add(txtBxOnMouseAreaInfo);

            int id_window = Render.Instance().AddWindow(wndTesteOnMouseMove.Size);


         
            AreaAtiva umaArea = new AreaAtiva(rectUmaAreaAtiva, this.OnMouseMove, this.OnMouseLeave, this.OnMouseEnter, this.OnMouseLeftClick, this.OnMouseRightClick);
            MouseSystem.Instance().areas.Add(umaArea);

            LoopGame.Instance().SizeScreen = wndTesteOnMouseMove.Size;
            LoopGame.Instance().fps = 48.0;
    
            LoopGame.Instance().AddUpdateMethod(this.UpdateMouseState); // registra no loop o update de informe da posição do estado.

            wndTesteOnMouseMove.Show();
            LoopGame.Instance().Run();

        } // TesteOnMouse()

        private void UpdateMouseState()
        {
            if (this.tempoDeAtualizacaoMouseState.IsTimeToAct())
            {
                this.txtBxOnMousePosition.Text = wndTesteOnMouseMove.PointToClient(Cursor.Position).ToString();
                this.txtBxOnMousePosition.Refresh();
            } // if
        } // UpdateMouseState()
        private void WndTesteOnMouseMove_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control c in this.wndTesteOnMouseMove.Controls)
                c.Refresh();
            if (this.imgAreaAtiva != null)
                e.Graphics.DrawImage(this.imgAreaAtiva, this.locationArea);
        } // WndTesteOnMouseMove_Paint()

        private void OnMouseMove()
        {
            txtBxOnMouseTeste.Text = "mouse move";
            txtBxOnMouseTeste.Refresh();
        }
        private void OnMouseEnter()
        {
            txtBxOnMouseTeste.Text="mouse enter";
            txtBxOnMouseTeste.Refresh();
        }

        private void OnMouseLeave()
        {
            txtBxOnMouseTeste.Text = "mouse leave";
            txtBxOnMouseTeste.Refresh();
        }

        private void OnMouseLeftClick()
        {
            txtBxOnMouseTeste.Text = "mouse left click";
            txtBxOnMouseTeste.Refresh();
        }

        private void OnMouseRightClick()
        {
            txtBxOnMouseTeste.Text = "mouse right click";
            txtBxOnMouseTeste.Refresh();
  
        }

        vetor2 ObjectDeslocamento()
        {
            return new vetor2(+0, +15);
        }

        private void TesteParalaxSystem(Assercoes assercao)
        {

            int id_window = Render.Instance().AddWindow(new Size(800, 600));


            Bitmap imagemFundo1 = new Bitmap("backgrounds\\background1.jpg");
            Bitmap imagemFundo2 = new Bitmap("backgrounds\\background2.jpg");
            Bitmap imagemFundo3 = new Bitmap("backgrounds\\background3.jpg");

            Paralax umParalax = new Paralax(new Point(0, 0), new Size(800,600), id_window, ObjectDeslocamento, new List<Bitmap>() { imagemFundo1, imagemFundo3 });

            LoopGame.Instance().RegistryObject(umParalax.Update, umParalax.Draw);
            LoopGame.Instance().Run();

        } // TesteParalaxSystem()

   


        private void TesteLabelInfo(Assercoes assercao)
        {

            int id_window = Render.Instance().AddWindow(new Size(800, 600));

            LabelInfo label = new LabelInfo("+1 food", 15.0f, new vetor2(50, 50), id_window);

            LoopGame.Instance().fps = 48.0;
            LoopGame.Instance().SizeScreen = new Size(800, 600);
            LoopGame.Instance().AddDrawMehod(label.Draw);


            LoopGame.Instance().Run();
        }

        MyImage imageSdl;

        private void TesteRotacaoImagens(Assercoes assercao)
        {
            Bitmap imagem = new Bitmap("StarShipMain.png");
            imagem = new Bitmap(imagem, new Size(32, 32));


            Form wndTesteRotacao = new Form()
            {
                Location = new Point(0, 0),
                Size = new Size(800, 600)
            };

            int id_window = Render.Instance().AddWindow(wndTesteRotacao.Size);



            this.imageSdl = new MyImage(imagem, imagem.Size);
            this.imageSdl.Rotate(-45.0);
            wndTesteRotacao.Paint += WndTesteRotacao_Paint;
            wndTesteRotacao.ShowDialog();
        } // TesteRotacaoImagens()

        private void WndTesteRotacao_Paint(object sender, PaintEventArgs e)
        {
            if (this.imageSdl != null)
                e.Graphics.DrawImage(this.imageSdl.GetGDIBitmap(), new PointF(100,100));
        } // WndTesteRotacao_Paint()

        public CorpoTestes()
        {
            Teste testeParaRotacaoDeImagem_teste = new Teste(this.TesteRotacaoImagens, "teste para rotação de imagem.");
            Teste testesParaBoxInformacao_teste = new Teste(this.TesteBoxInfo, "teste funcional para BoxInfo.");
            Teste testeBarraDeMensagens_teste = new Teste(this.TesteBarraDeMensagens, "teste funcional para barra de mensagens.");
            Teste testeControlTime_teste = new Teste(this.TesteControlTime, "teste funcional para control de tempo.");
            Teste testeModificadorDeCores_teste = new Teste(this.TesteChangerColors, "teste funcional para modificador de cores em uma imagem.");
            Teste testeModificadorDeCores_2 = new Teste(this.TesteChangerColors_2, "teste funcional para modificação de vários clusters");
            Teste testeBarraDeMensagensDoisEstagios_teste = new Teste(this.TesteBarra2Estagios, "teste funcional para uma barra de mensagens de 2 estágios.");
            Teste testeMapaEmEscala_teste = new Teste(this.TesteMapaEmScala, "teste para mapa em escala.");
            Teste testeHUDradar_teste = new Teste(this.testeHudRadar, "teste funcional da classe space radar.");
            Teste testeIconAvatar_teste = new Teste(this.TesteIconAvatar, "teste funcional para icon avatar componente.");
            Teste testeJoystick_teste = new Teste(this.TesteJoystick, "teste funcional para joystick vetorial.");
            Teste testeObtemAnguloGraus_teste = new Teste(this.TesteObtemAngulo, "teste automatizado para leitura de ângulo de vetor 2D");
            Teste testeCustomColors_teste = new Teste(this.TesteControlCustomColors, "teste funcional de control com cores customizadas.");
            Teste testeStateMachine_teste = new Teste(this.TesteStateMachine, "teste funcional para classe State Machine.");
            Teste testeOnMouse_teste = new Teste(this.TesteOnMouse, "teste funcional para a class OnMouse.");
            Teste testeSistemaParalax_teste = new Teste(this.TesteParalaxSystem, "teste funcional para a class paralax.");
            Teste testeLabelInfo_teste = new Teste(this.TesteLabelInfo, "teste funcional para a class LabelInfo.");


            ContainerTestes ContainerTestes = new ContainerTestes(testeHUDradar_teste);         
            ContainerTestes.ExecutaTestesEExibeResultados(); 
            
        } // ContainerTestes()


    } // class CorpoTestes
} // namespace ModuloTESTES

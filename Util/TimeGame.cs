using System;

namespace Classes3D.Util
{

    /// <summary>
    /// implementa o gerenciamento do tempo no jogo.
    /// </summary>
    public class TimeGame
    {

        private DateTime dtLastCicle = DateTime.Now;
        private double dtMillisecoundsPast = 0.0;
        /// <summary>
        /// obtém o tempo absoluto em milisegundos.
        /// </summary>
        /// <returns></returns>
        public double GetTime()
        {
            DateTime dt = DateTime.Now;
            double timePassed = (dt - dtLastCicle).TotalMilliseconds;
            dtLastCicle = DateTime.Now;
            return timePassed;
        } //GetTime()

        /// <summary>
        /// obtém o tempo passado entre o último ciclo do jogo e o ciclo atual.
        /// </summary>
        /// <returns></returns>
        public double GetElapsedTime()
        {
            double TimeNow = GetTime();
            double timePassed = TimeNow - dtMillisecoundsPast;
            dtMillisecoundsPast = GetTime();
            return timePassed;
        } // GetElapsedTime()
    }  // class TimeGame


    /// <summary>
    /// classe que encapsula o tempo de reação para determinada atividade. É semelhante ao [TimeGame.GetElapsedTime()],
    /// mas contém algumas funcionalidade a mais, além de esconder as contas feitas do tempo percorrido.
    /// </summary>
    public class TimeReaction
    {
        private double TimeBeginCycle;
        public double TimeFire;
        public static double FPS=60.0;
        private TimeGame time = new TimeGame();
        private double timeAcumulated = 0.0;
        
        public TimeReaction(double timeAcao)
        {
            this.TimeFire = timeAcao;
            this.TimeBeginCycle = time.GetTime();
        }
        /// <summary>
        /// retorna [true] se passou o tempo de reação, [false] se não passou o tempo de reação.
        /// </summary>
        /// <returns>retorna [true] para ocorreu o tempo de reagir, [false] se não.</returns>
        public bool IsTimeToAct()
        {
            this.timeAcumulated+= time.GetTime();
            if ((this.timeAcumulated+this.TimeBeginCycle)>=this.TimeFire)
            {
                this.timeAcumulated = this.TimeFire - (this.timeAcumulated + this.TimeBeginCycle);
                TimeBeginCycle = time.GetTime();
                return true;
            } // if
            return false;
        } //IsTimeToAct()

        /// <summary>
        /// retorna o tempo passado desde o último ciclo do game.É uma funcionalidade idêntica ao [TimeGame.GetElapsedTime()]
        /// </summary>
        /// <returns></returns>
        public double TimeElapsed()
        {
            double timeAtual = time.GetTime();
            double timeDelta = timeAtual - TimeBeginCycle;
            TimeBeginCycle = time.GetTime();
            return timeDelta;
        } // TimeElapsed()
    }// class TimeReaction

  
} // namespace

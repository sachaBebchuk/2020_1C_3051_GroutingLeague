﻿using System.Drawing;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Text;
using Microsoft.DirectX.DirectInput;
using TGC.Core.SceneLoader;
using System.ComponentModel;
using BulletSharp.Math;
using TGC.Core.Mathematica;
using System.Collections.Generic;

namespace TGC.Group.Model
{
    class EscenaMenu : Escena
    {
        private TgcText2D texto;
        private TgcMesh cancha;

        private List<Jugador> jugadores = new List<Jugador>();
        private int jugadorActivo = 0;
        public EscenaMenu(TgcCamera Camera, string MediaDir, TgcText2D DrawText, float TimeBetweenUpdates, TgcD3dInput Input) : base(Camera, MediaDir, DrawText, TimeBetweenUpdates, Input)
        {
            texto = new TgcText2D();
            texto.Align = TgcText2D.TextAlign.CENTER;
            texto.Position = new Point(0, D3DDevice.Instance.Height / 2);
            texto.Color = Color.White;
            texto.changeFont(new Font("TimesNewRoman", 50, FontStyle.Bold));
            texto.Text = "Espacio para empezar";



            TgcScene escena = new TgcSceneLoader().loadSceneFromFile(MediaDir + "Cancha-TgcScene.xml");

            cancha = escena.Meshes[0];
            Camera.SetCamera(new TGCVector3(20, 10, -20), new TGCVector3(0, 5,-10));
            initJugadores(escena);
        }


        private void initJugadores(TgcScene escena)
        {
            Jugador auto = new Jugador("Auto", escena.Meshes[2], new TGCVector3(0 , 5, 0), new TGCVector3(-.5f, 0, 0));
            Jugador tractor = new Jugador("Tractor", escena.Meshes[5], new TGCVector3(0, 5, 0), new TGCVector3(-.5f, 0, 0));
            Jugador patrullero = new Jugador("Patrullero", escena.Meshes[3], new TGCVector3(0, 5, 0), new TGCVector3(-.5f, 0, 0));
            Jugador tanque = new Jugador("Tanque", escena.Meshes[4], new TGCVector3(0, 5, 0), new TGCVector3(-.5f, 0, 0));

            jugadores.Add(auto);
            jugadores.Add(tractor);
            jugadores.Add(patrullero);
            jugadores.Add(tanque);
        }
        public override void Dispose()
        {
            texto.Dispose();
        }

        public override void Render()
        {
            texto.render();
            cancha.Render();
            jugadores[jugadorActivo].Render();
        }

        public override Escena Update(float ElapsedTime)
        {
            if (Input.keyDown(Key.Space))
            {
                return CambiarEscena(new EscenaJuego(Camera, MediaDir, DrawText, TimeBetweenUpdates, Input, jugadores, jugadores[jugadorActivo]));
            }
            if (Input.keyDown(Key.RightArrow))
            {
                siguienteJugador();
            }
            return this;
        }

        private void siguienteJugador()
        {
            if (++jugadorActivo == jugadores.Count)
                jugadorActivo = 0;
        }
    }
}

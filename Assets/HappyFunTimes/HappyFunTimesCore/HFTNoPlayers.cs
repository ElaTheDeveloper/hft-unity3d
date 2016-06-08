/*
 * Copyright 2014, Gregg Tavares.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 *     * Redistributions of source code must retain the above copyright
 * notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above
 * copyright notice, this list of conditions and the following disclaimer
 * in the documentation and/or other materials provided with the
 * distribution.
 *     * Neither the name of Gregg Tavares. nor the names of its
 * contributors may be used to endorse or promote products derived from
 * this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 * OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using HappyFunTimes;

namespace HappyFunTimes
{
    // For making creations that don't use players like
    // multi-screen installations. The only reason to use
    // this over PlayerSpawner is that PlayerSpawner
    // will try to spawn prefabs and complain.
    [AddComponentMenu("HappyFunTimes/HFTNoPlayers")]
    public class HFTNoPlayers : MonoBehaviour
    {
        public HFTGameOptions happyfuntimesOptions = new HFTGameOptions();

        public GameServer server
        {
            get
            {
                return m_server;
            }
        }

        public void StartHappyFunTimes()
        {
            StartConnection();
        }

        public void StopHappyFunTimes()
        {
            Cleanup();
        }

        void StartConnection()
        {
            if (!m_started)
            {
                enabled = true;
                m_started = true;
                m_hftManager.Start(m_options, gameObject);
            }
        }

        void FailedToStart(object sender, System.EventArgs e)
        {
            m_log.Error("could not connect to server:");
        }

        void StartGameServer(object sender, System.EventArgs e)
        {
            m_server.Init();
        }

        void Awake()
        {
            m_connectToServerOnStart = enabled;
            m_options = new HFTRuntimeOptions(happyfuntimesOptions);

            m_server = new GameServer(m_options, gameObject);
            m_server.OnConnect += Connected;
            m_server.OnDisconnect += Disconnected;

            m_hftManager = new HFTManager();
            m_hftManager.OnReady += StartGameServer;
            m_hftManager.OnFail += FailedToStart;
        }

        void Start()
        {
            if (m_connectToServerOnStart)
            {
                StartConnection();
            }
        }

        void Connected()
        {
        }

        void Disconnected()
        {
        }

        void Cleanup()
        {
            if (m_started)
            {
                enabled = false;
                m_started = false;

                if (m_server != null)
                {
                    m_server.Close();
                }

                if (m_hftManager != null)
                {
                    m_hftManager.Stop();
                }
            }
        }

        void OnDestroy()
        {
            Cleanup();
        }

        void OnApplicationExit()
        {
            Cleanup();
        }

        public GameServer GameServer
        {
            get
            {
                return m_server;
            }
            private set
            {
            }
        }

        private bool m_started;
        private bool m_connectToServerOnStart;
        private GameServer m_server;
        private HFTManager m_hftManager;
        private HFTLog m_log = new HFTLog("HFTNoPlayers");
        private HFTRuntimeOptions m_options;
    };

}   // namespace HappyFunTimes

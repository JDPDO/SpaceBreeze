using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API.Entities;
using ElectronNET.API;
using Microsoft.AspNetCore.Html;

namespace JDPDO.Mittuntur.UI.Models
{
    /// <summary>
    /// Provides inter process communication ability.
    /// </summary>
    public static class Ipc
    {
        /// <summary>
        /// Creates the sender js element for View Page.
        /// </summary>
        /// <param name="triggerElementId"></param>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public static IHtmlContent RendererSendOnTrigger(string channel, string triggerElementId, string message = "", string trigger = "click")
        {
            return GetScriptElement(
                $"const {{ ipcRenderer }} = require(\"electron\");" +
                $"document.getElementById(\"{triggerElementId}\").addEventListener(\"{trigger}\", () => {{" +
                $"ipcRenderer.send(\"{channel}\", \"{message}\");}})"
                );
        }

        /// <summary>
        /// Adds to renderer process a channel to listen with listener.
        /// Defines in which certain element message should be printed.
        /// </summary>
        /// <param name="elementId">Id of affected HTML element.</param>
        /// <param name="channel">Channel to listen.</param>
        /// <returns></returns>
        public static IHtmlContent RendererOnIpc(string channel, string elementId)
        {
            return GetScriptElement($"ipcRenderer.on( '{channel}', (event, arg) => {{ document.getElementById('{elementId}').innerHTML = arg }} )");
        }

        /// <summary>
        /// Returnes a filled script element.
        /// </summary>
        /// <param name="jsString">A string containing JavaScript code.</param>
        /// <returns></returns>
        public static IHtmlContent GetScriptElement(string jsString)
        {
            IHtmlContentBuilder builder = new HtmlContentBuilder();
            HtmlString html = new HtmlString(
                $"<script> {jsString} </script>");
            builder.AppendHtml(html);
            return builder;

        }

        /// <summary>
        /// Sends a message to a Electron renderer process.
        /// </summary>
        /// <param name="channel">Channel to write on.</param>
        /// <param name="message">Message to send.</param>
        /// <param name="window">Reciver window of message. If null the mainWindow is used.</param>
        /// <returns></returns>
        public static void Send(string channel, string message, BrowserWindow window = null)
        {
            if (window == null) window = Electron.WindowManager.BrowserWindows.First();
            Electron.IpcMain.Send(window, channel, message);
        }

        /// <summary>
        /// Adds to the main process a channel to listen with listener and
        /// defines the reply with already defined respond data array.
        /// </summary>
        /// <param name="channel">Channel to listen and write on.</param>
        /// <param name="window">Reciver window of message. If null the mainWindow is used.</param>
        /// <param name="data">To sending data.</param>
        public static void On(string channel, string responseChannel, BrowserWindow window = null, params object[] data)
        {
            Electron.IpcMain.On(channel, (arg) =>
            {
                if (window == null) window = Electron.WindowManager.BrowserWindows.First();
                Electron.IpcMain.Send(window, responseChannel, data);
            });
        }

        /// <summary>
        /// Adds to the main process a channel to listen with listener and
        /// defines the reaction.
        /// </summary>
        /// <param name="channel">Channel to listen.</param>
        /// <param name="action">Task to execute when event rises.</param>
        public static void On(string channel, Action action)
        {
            Electron.IpcMain.On(channel, (args) =>
            {
                action();
            });
        }

        /// <summary>
        /// Adds to calling renderer process a script providing ipc functionality.
        /// Adds to main process a ipc event handler.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="triggerElementId"></param>
        /// <param name="triggerMessage"></param>
        /// <param name="elementId"></param>
        /// <param name="action"></param>
        /// <param name="trigger"></param>
        public static void AddChannel(
            string channel, 
            string triggerElementId, 
            string triggerMessage, 
            Action action,
            string elementId = null,
            string trigger = "click")
        {
            // Add event Ipc functionality to current View.
            RendererSendOnTrigger(channel, triggerElementId, triggerMessage, trigger);
            if (elementId != null) RendererOnIpc(channel, elementId);

            // Add event Ipc fuctionality to main process.
            On(channel, action);
        }

        /// <summary>
        /// Adds to calling renderer process a script providing ipc functionality.
        /// Adds to main process a ipc event handler responding with a defined data array to a defined window process.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="triggerElementId"></param>
        /// <param name="triggerMessage"></param>
        /// <param name="elementId"></param>
        /// <param name="window"></param>
        /// <param name="trigger"></param>
        /// <param name="data"></param>
        public static void AddChannel(
            string channel,
            string responseChannel,
            string triggerElementId, 
            string triggerMessage, 
            string elementId = null, 
            BrowserWindow window = null, 
            string trigger = "click", 
            params object[] data)
        {
            // Add event Ipc functionality to current View.
            RendererSendOnTrigger(channel, triggerElementId, triggerMessage, trigger);
            if (elementId != null) RendererOnIpc(channel, elementId);

            // Add event Ipc fuctionality to main process for defined request.
            if (window == null) window = Electron.WindowManager.BrowserWindows.First();
            On(channel, responseChannel, window, data);
        }
    }
}

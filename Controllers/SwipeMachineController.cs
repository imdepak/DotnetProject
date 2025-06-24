using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyDotnetProject.Models;
using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class SwipeMachineController : ControllerBase
    {
        private readonly string _posIp = "localhost"; // Update with POS machine IP
        private readonly int _posPort = 23001; // Update with the correct port

        [HttpGet]
        public String Check()
        {
            return "I m From WEb APi";
        }

        [HttpGet("send-command")]
        public async Task<IActionResult> SendCommand()
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    Console.WriteLine($"🔵 Connecting to TCP Server at {_posIp}:{_posPort}...");
                    await client.ConnectAsync(_posIp, _posPort);
                    Console.WriteLine("✅ Connected!");

                    using (NetworkStream stream = client.GetStream())
                    {
                        string command = "YOUR_COMMAND_HERE";
                        byte[] data = Encoding.ASCII.GetBytes(command);

                        Console.WriteLine($"📤 Sending command: {command}");
                        await stream.WriteAsync(data, 0, data.Length);

                        // Receive response
                        byte[] buffer = new byte[1024];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                        Console.WriteLine($"📨 Received response from POS: {response}");

                        return Ok(new { message = "Response from POS", data = response });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("receive-data")]
        public IActionResult ReceiveData([FromBody] string message)
        {
            try
            {
                Console.WriteLine($"📩 Data received from Console App: {message}");
                return Ok(new { status = "Success", receivedMessage = message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in Web API: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("check-connection")]
        public async Task<IActionResult> CheckConnection()
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    Console.WriteLine($"🔵 Checking connection to TCP Server at {_posIp}:{_posPort}...");
                    await client.ConnectAsync(_posIp, _posPort);
                    Console.WriteLine("✅ Connected!");

                    using (NetworkStream stream = client.GetStream())
                    {
                        string pingMessage = "ping";
                        byte[] data = Encoding.ASCII.GetBytes(pingMessage);

                        Console.WriteLine($"📤 Sending handshake message: {pingMessage}");
                        await stream.WriteAsync(data, 0, data.Length);

                        // Receive confirmation response
                        byte[] buffer = new byte[256];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                        Console.WriteLine($"📨 Server response: {response}");
                        return Ok(new { message = "Connection Check", serverResponse = response });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Connection check failed: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }


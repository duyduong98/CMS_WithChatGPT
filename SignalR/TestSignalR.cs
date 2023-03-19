using Microsoft.AspNetCore.SignalR;
using ProjectCMS.Models;

namespace ProjectCMS.SignalR
{
    public class TestSignalR : Hub
    {
         public async Task UpdateInfomation(List<Category> categories)
         {
            await Clients.All.SendAsync("Infomation Update", categories);

            return;
         }
    }
}

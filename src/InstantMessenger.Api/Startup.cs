using InstantMessenger.Friendships;
using InstantMessenger.Groups;
using InstantMessenger.Identity;
using InstantMessenger.PrivateMessages;
using InstantMessenger.Profiles;
using InstantMessenger.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InstantMessenger.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityModule()
                .AddProfilesModule()
                .AddFriendshipsModule()
                .AddPrivateMessagesModule()
                .AddGroupsModule()
                .AddSharedModule();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseIdentityModule()
                .UseSharedModule();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
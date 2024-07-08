using FrappeQbwcService.FrappeModels;
using Microsoft.EntityFrameworkCore;
using QbSync.QbXml.Objects;
using System.Threading.Tasks;
using WebApplication.Sample.Db;

namespace WebApplication.Sample.Extensions
{
    public static class QbSettingExtensions
    {
        public static async Task SaveIfNewerAsync(this ApplicationDbContext applicationDbContext, string setting, DATETIMETYPE? moment)
        {
            if (moment != null)
            {
                var savedSetting  = Manager.GetManager().GetSetting(setting);
               // var savedSetting = await applicationDbContext.QbSettings.FirstOrDefaultAsync(m => m.setting_name == setting);
                var existingDateTimeType = DATETIMETYPE.ParseOrDefault(savedSetting?.value, DATETIMETYPE.MinValue);
                bool isCreate = false;
                if (moment > existingDateTimeType)
                {
                    if (savedSetting == null)
                    {
                        savedSetting = new QbSetting
                        {
                            setting_name = setting
                        };
                        isCreate = true;
                    }

                    savedSetting.value = moment.ToString();
                    Manager.GetManager().SaveSetting(savedSetting, isCreate);
                }
            }
        }
    }
}

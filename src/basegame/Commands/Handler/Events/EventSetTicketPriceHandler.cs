using ColossalFramework.UI;
using CSM.API.Commands;
using CSM.API.Helpers;
using CSM.BaseGame.Commands.Data.Events;
using CSM.BaseGame.Helpers;

namespace CSM.BaseGame.Commands.Handler.Events
{
    public class EventSetTicketPriceHandler : CommandHandler<EventSetTicketPriceCommand>
    {
        protected override void Handle(EventSetTicketPriceCommand command)
        {
            IgnoreHelper.Instance.StartIgnore();

            ref EventData eventData = ref EventManager.instance.m_events.m_buffer[command.Event];
            eventData.Info.m_eventAI.SetTicketPrice(command.Event, ref eventData, command.Price);

            Type[] panelTypes =
            {
                typeof(FootballPanel),
                typeof(FestivalPanel),
                typeof(VarsitySportsArenaPanel)
            };

            foreach (Type panelType in panelTypes)
            {
                if (InfoPanelHelper.IsEventBuilding(panelType, command.Event, out WorldInfoPanel panel))
                {
                    UISlider slider = ReflectionHelper.GetAttr<UISlider>(panel, "m_TicketPriceSlider")
                                   ?? ReflectionHelper.GetAttr<UISlider>(panel, "m_ticketPriceSlider");

                    if (slider != null)
                    {
                        SimulationManager.instance.m_ThreadingWrapper.QueueMainThread(() =>
                        {
                            slider.value = command.Price / 100f;
                        });
                    }
                }
            }

            IgnoreHelper.Instance.EndIgnore();
        }
    }
}

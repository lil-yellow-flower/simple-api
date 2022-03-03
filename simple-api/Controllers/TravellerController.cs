using Microsoft.AspNetCore.Mvc;
using simple_api.Helpers;

namespace simple_api.Controllers
{
    [Route("/")]
    public class TravellerController : ControllerBase
    {
        private readonly BoardingCardsHelper _boardingCardsHelper;

        public TravellerController(BoardingCardsHelper boardingCardsHelper)
        {
            _boardingCardsHelper = boardingCardsHelper;
        }

        [HttpPost]
        public ActionResult<object> Post([FromBody] List<string> collection)
        {
            var result = _boardingCardsHelper.OrganizeBoardingCards(collection);
            if(result.Any())
            {
                return Ok(result);
            }

            return BadRequest("Input is in incorrect format or lacks valid starting point... (ง •̀_•́)ง");
        }
    }
}

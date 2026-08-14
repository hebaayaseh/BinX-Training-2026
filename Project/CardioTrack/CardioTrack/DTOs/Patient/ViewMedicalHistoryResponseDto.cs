using CardioTrack.Models;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.DTOs.Patient
{
    public class ViewMedicalHistoryResponseDto
    {
        public List<MidicalHistoyDto> Midicals { get; set; }
    }
}

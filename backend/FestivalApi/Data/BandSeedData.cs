using FestivalApi.Models;

namespace FestivalApi.Data;

public static class BandSeedData
{
    public static IReadOnlyList<Band> All { get; } =
    [
        new Band
        {
            Id = 1,
            Name = "Knife Sticking Head",
            Bio = @"Knife Sticking Head là một ban nhạc hardcore old-school của Việt Nam, có nguồn gốc từ Sài Gòn, được thành lập vào tháng 4 năm 2014.

Âm nhạc của Knife Sticking Head chủ yếu được xếp vào thể loại hardcore old-school. Trong suốt hơn mười năm hoạt động, phong cách của họ đã dần phát triển, kết hợp thêm các yếu tố từ những nhánh metal khác và đưa vào nhiều giai điệu hơn. Định hướng âm nhạc của ban nhạc thay đổi theo từng bài hát, tùy thuộc vào thông điệp mà họ muốn truyền tải qua âm nhạc.

Thành viên:
● Nguyễn Chánh Hiệp  - rhythm guitarist
● Lê Hoàng Minh Quân - lead guitarist
● Quan Vĩnh Kiện - lead vocalist
● Nguyễn Huy Khiêm -  bassist
● Trương Kim Trọng - drummer",
            HeroUrl = "/assets/bands/ksh/hero.jpeg",
            LogoUrl = "/assets/bands/ksh/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 1
        },
         new Band
        {
            Id = 2,
            Name = "Morning Wait",
            Bio = "Knife Sticking Head is a Vietnamese oldschool hardcore band originating from Saigon, Vietnam, established in April 2014. The current lineup comprises lead vocalist Quan Vinh Kien, drummer Truong Kim Trong (Rohan), lead guitarist Le Hoang Minh Quan (Rin), rhythm guitarist Nguyen Chanh Hiep (Bill) and bassist Nguyen Huy Khiem. The band has released 1 EP and 4 singles, with their most recent music video, \"From The Inside\", released on Apr 19, 2024.",
            HeroUrl = "/assets/bands/morningwait/hero.jpg",
            LogoUrl = "/assets/bands/morningwait/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 2
        },
        new Band
        {
            Id = 3,
            Name = "Amongst The Fallen",
            Bio = "",
            HeroUrl = "/assets/bands/amongstthefallen/hero.jpg",
            LogoUrl = "/assets/bands/amongstthefallen/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 3
        },
        new Band
        {
            Id = 4,
            Name = "Black Industry",
            Bio = "",
            HeroUrl = null,
            LogoUrl = "/assets/bands/blackindustry/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 4
        },
        new Band
        {
            Id = 5,
            Name = "Cutlon",
            Bio = "",
            HeroUrl = null,
            LogoUrl = "/assets/bands/cutlon/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 5
        },
        new Band
        {
            Id = 6,
            Name = "Die So Far",
            Bio = "",
            HeroUrl = "/assets/bands/diesofar/hero.JPG",
            LogoUrl = "/assets/bands/diesofar/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 6
        },
        new Band
        {
            Id = 7,
            Name = "Elbow Drop",
            Bio = "",
            HeroUrl = "/assets/bands/elbowdrop/hero.jpg",
            LogoUrl = "/assets/bands/elbowdrop/logo1.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 7
        },
        new Band
        {
            Id = 8,
            Name = "Empathize",
            Bio = "",
            HeroUrl = "/assets/bands/empathize/hero.jpg",
            LogoUrl = "/assets/bands/empathize/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 8
        },
        new Band
        {
            Id = 9,
            Name = "Hypertension",
            Bio = "",
            HeroUrl = null,
            LogoUrl = "/assets/bands/hypertension/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 9
        },
        new Band
        {
            Id = 10,
            Name = "Kinh",
            Bio = "",
            HeroUrl = "/assets/bands/kinh/hero.jpg",
            LogoUrl = "/assets/bands/kinh/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 10
        },
        new Band
        {
            Id = 11,
            Name = "Nhao",
            Bio = "",
            HeroUrl = "/assets/bands/nhao/hero.jpeg",
            LogoUrl = "/assets/bands/nhao/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 11
        },
        new Band
        {
            Id = 12,
            Name = "Resurged Temptation",
            Bio = @"Resurged Temptation là một ban nhạc metalcore Việt Nam đến từ Sài Gòn, Việt Nam, được thành lập vào cuối năm 2022.

Chất liệu của Resurged Temptation là cảm hứng từ thể loại metalcore. Tuy nhiên, phong cách âm nhạc tổng thể của ban nhạc được các thành viên miêu tả là sự kết hợp giữa metalcore, melodic death và groove metal. Đặc trưng phong cách của họ bao gồm việc sử dụng các đoạn breakdowns, growling vocals, clean vocals và các đoạn riff hơi hướng groove.

Thành viên:
●	Nguyễn Thành Các - guitar rhythm 
●	Trần Hoàng Phúc - guitar lead 
●	Đinh Bảo Nam - lead vocalist
●	Trương Kim Trọng - bass, backing vocal
●	Võ Phúc Hoan - drummer",
            HeroUrl = "/assets/bands/resurgedtemptation/hero.jpg",
            LogoUrl = "/assets/bands/resurgedtemptation/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 12
        },
        new Band
        {
            Id = 13,
            Name = "Song Am",
            Bio = "",
            HeroUrl = "/assets/bands/songam/hero.png",
            LogoUrl = "/assets/bands/songam/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 13
        },
        new Band
        {
            Id = 14,
            Name = "Thanh Duc",
            Bio = "",
            HeroUrl = "/assets/bands/thanhduc/hero.jfif",
            LogoUrl = "/assets/bands/thanhduc/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 14
        },
        new Band
        {
            Id = 15,
            Name = "Under Pressure",
            Bio = "",
            HeroUrl = "/assets/bands/underpressure/hero.jpg",
            LogoUrl = "/assets/bands/underpressure/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 15
        },
        new Band
        {
            Id = 16,
            Name = "1818",
            Bio = "",
            HeroUrl = "/assets/bands/1818/hero.png",
            LogoUrl = null,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 16
        },
        new Band
        {
            Id = 17,
            Name = "7 Uppercuts",
            Bio = "",
            HeroUrl = "/assets/bands/7uppercuts/hero.png",
            LogoUrl = null,
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 17
        },
        new Band
        {
            Id = 18,
            Name = "9xactly",
            Bio = "",
            HeroUrl = "/assets/bands/9xactly/hero.png",
            LogoUrl = "/assets/bands/9xactly/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 18
        },
        new Band
        {
            Id = 19,
            Name = "Diarsia",
            Bio = "",
            HeroUrl = "/assets/bands/diarsia/hero.png",
            LogoUrl = "/assets/bands/diarsia/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 19
        },
        new Band
        {
            Id = 20,
            Name = "I'm Not Sure",
            Bio = "",
            HeroUrl = "/assets/bands/imnotsure/hero.png",
            LogoUrl = null,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 20
        },
        new Band
        {
            Id = 21,
            Name = "Infernal Chaos",
            Bio = "",
            HeroUrl = "/assets/bands/infernalchaos/hero.png",
            LogoUrl = null,
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 21
        },
        new Band
        {
            Id = 22,
            Name = "Longbez",
            Bio = "",
            HeroUrl = "/assets/bands/longbez/hero.png",
            LogoUrl = null,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 22
        },
        new Band
        {
            Id = 23,
            Name = "Surprise",
            Bio = "",
            HeroUrl = "/assets/bands/surprise/hero.png",
            LogoUrl = null,
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 23
        },
        new Band
        {
            Id = 24,
            Name = "Volcate",
            Bio = "",
            HeroUrl = "/assets/bands/volcate/hero.png",
            LogoUrl = null,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 24
        },
        new Band
        {
            Id = 25,
            Name = "CLB Guitar Go",
            Bio = "",
            HeroUrl = "/assets/bands/clbguitargo/hero.png",
            LogoUrl = "/assets/bands/clbguitargo/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 25
        }
    ];
}

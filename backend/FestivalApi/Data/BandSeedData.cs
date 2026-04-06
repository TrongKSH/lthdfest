using System.Collections.Generic;
using FestivalApi.Models;

namespace FestivalApi.Data;

public static class BandSeedData
{
    public static IReadOnlyList<Band> All { get; } = new List<Band>
    {
        #region Band 1 — Knife Sticking Head
        new Band
        {
            Id = 1,
            Name = "Knife Sticking Head",
            Bio = @"Knife Sticking Head là một ban nhạc hardcore old-school của Việt Nam, có nguồn gốc từ Sài Gòn, được thành lập vào tháng 4 năm 2014.

Ban nhạc đã phát hành đĩa đơn đầu tay “Do The Riot Things” vào ngày 7 tháng 1 năm 2015.

Sau thành công của sản phẩm đầu tay, họ tập trung sáng tác các ca khúc mới và tiếp tục ra mắt MV đầu tiên “This Is Home” vào ngày 16 tháng 8 năm 2017.

Ngày 25 tháng 4 năm 2018, ban nhạc phát hành EP đầu tiên mang tên “This Is Home”, gồm 7 ca khúc.

Vào tháng 10 năm 2018, ban nhạc thông báo tan rã do Huân (bass) và Trọng (trống) ra nước ngoài làm việc và học tập. Tuy nhiên, sau 5 tháng cùng với sự ủng hộ lớn từ cộng đồng, ban nhạc đã tái hợp với các thành viên mới là Phi (trống) và Nhựt (bass).

Ngày 12 tháng 12 năm 2020, ban nhạc phát hành MV mới mang tên “Respect” với đội hình mới.

Tháng 1 năm 2020, Trương Kim Trọng (tay trống cũ) quay trở lại ban nhạc, thay thế Phi.

Tháng 7 năm 2020, Le Hoàng Minh Quân gia nhập ban nhạc với vai trò guitarist và clean vocalist mới, thay thế Lâm Tân Khoa.

Sau khi ổn định đội hình, ban nhạc tiếp tục phát hành nhiều MV mới như “Holding Youth”, “Imagination”, và mới nhất là “From The Inside”.

Tháng 6 năm 2024, Nguyễn Huy Khiêm gia nhập ban nhạc, thay thế Trang Bùi Minh Nhựt ở vị trí bass.

Với đội hình hiện tại, ban nhạc tiếp tục thu âm các ca khúc mới và tập trung sáng tác nhạc gốc. Đĩa đơn mới nhất mang tên “6789” dự kiến sẽ sớm được phát hành.

Âm nhạc của Knife Sticking Head chủ yếu được xếp vào thể loại hardcore old-school. Trong suốt hơn mười năm hoạt động, phong cách của họ đã dần phát triển, kết hợp thêm các yếu tố từ những nhánh metal khác và đưa vào nhiều giai điệu hơn. Định hướng âm nhạc của ban nhạc thay đổi theo từng bài hát, tùy thuộc vào thông điệp mà họ muốn truyền tải qua âm nhạc.

Thành viên:
• Nguyễn Chánh Hiệp  - rhythm guitarist
• Lê Hoàng Minh Quân - lead guitarist
• Quan Vĩnh Kiện - lead vocalist
• Nguyễn Huy Khiêm -  bassist
• Trương Kim Trọng - drummer",
            BioEn = @"Knife Sticking Head is a Vietnamese old-school hardcore band from Saigon, founded in April 2014.

The band released its debut single ""Do The Riot Things"" on January 7, 2015, followed by its first music video ""This Is Home"" on August 16, 2017 and the first EP ""This Is Home"" (7 tracks) on April 25, 2018.

In October 2018, the band announced a breakup due to key members moving abroad, but reunited five months later with new members. Since then, they have released new works including ""Respect"", ""Holding Youth"", ""Imagination"", and most recently ""From The Inside"". The latest single ""6789"" is expected soon.

Over more than ten years, their sound has evolved from old-school hardcore by blending elements from other metal subgenres while keeping a strong song-by-song message-driven direction.

Members:
• Nguyen Chanh Hiep - rhythm guitarist
• Le Hoang Minh Quan - lead guitarist
• Quan Vinh Kien - lead vocalist
• Nguyen Huy Khiem - bassist
• Truong Kim Trong - drummer",
            HeroUrl = "/assets/bands/ksh/hero.jpeg",
            LogoUrl = "/assets/bands/ksh/logo.png",
            IsFeaturedOnHome = true,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 1
        },
         #endregion
        #region Band 2 — Morning Waits
         new Band
        {
            Id = 2,
            Name = "Morning Waits",
            Bio = @"Morning Waits được thành lập từ năm 2013, là những mảnh ghép từ những band nhạc rock khá có tiếng trong giới underground. Họ là những người anh em chơi với nhau và từ lâu vẫn luôn cuốn trong dòng chảy của NuMetal, MetalCore, Melodic Death Metal. Họ khao khát được cùng nhau chơi một thứ nhạc mạnh mẽ, giàu giai điệu và trình diễn live thật hiệu quả. Sau một thời gian thử nghiệm với nhiều sản phẩm và các cuộc thi lớn nhỏ, họ cùng đi tới quyết định sẽ lựa chọn dòng nhạc melodic metalcore mạnh mẽ, ảnh hưởng trực tiếp từ những thần tượng lớn trên thế giới như Killswitch Engage, As I Lay Dying, While She Sleeps, Parkway Drive.

Thời điểm Morning Waits được biết đến nhiều nhất là khi họ ra mắt single ""The Lasting Memories"", một bản ballad vô cùng tình cảm nhưng cũng rất mạnh mẽ. Single đã giúp ban nhạc có được sự chú ý của đạo diễn Quốc Trung, tổng đạo diễn của Rockstorm 2014 – Hào Khí Đông A, và Morning Waits là ban nhạc metal duy nhất có mặt trong 3 show Rockstorm liên tiếp tại Hải Phòng, Huế và Hà Nội. Morning Waits cũng là cái tên được nhắc tới rất nhiều trong hai kỳ HardCore United 2015 và 2016, một sự kiện tập hợp rất nhiều các ban nhạc chơi thể loại metalcore và thể nghiệm khác trên cả nước và các đại diện ưu tú trong khu vực.

Sau một thời gian vắng bóng để tập trung cho việc thu và hoàn thiện album đầu tay, band nhạc đã ra mắt ""The Travelers"" thông qua DEV record label vào năm 2019 và nhận được rất nhiều đánh giá tích cực của cộng đồng. Album ghi lại quá trình chiến đấu của chính bản thân các thành viên trong ban nhạc để tiếp tục bước đi với chính niềm đam mê của mình, cũng là cách mà ban nhạc ghi lại những dấu ấn trong cuộc sống của từng cá nhân trong suốt những năm gắn bó. Trong năm này, họ cũng đã tham gia show Rock'n'Share của đạo diễn Trần Trung Lĩnh cùng với những tên tuổi hàng đầu của Rock Việt.

Đội hình hiện tại của Morning Waits bao gồm:
• Nguyễn Trung Anh
• Nguyễn Quang
• Hoàng Việt Anh
• Trần Bảo Lâm
• Hà Đăng Đức

Nếu để hỏi tại sao các bạn nên nghe nhạc của Morning Waits, thì đó là sự nhiệt huyết, là những câu singalong sáng sủa trên nền những câu riff nhanh và giàu giai điệu của guitar, là sự dồn dập của drum và bass — tất cả hòa quyện lại thành một thứ âm nhạc mà mỗi khi nổi lên là không thể đứng yên được.",
            BioEn = @"Morning Waits was formed in 2013, brought together from members of several well-known bands in the underground rock scene. They are bandmates who have played together for years, always immersed in the currents of Nu-Metal, Metalcore, and Melodic Death Metal. They share a desire to play music that is powerful and melodic while delivering an impactful live show. After experimenting with various recordings and competitions, they settled on a sound rooted in heavy melodic metalcore, directly influenced by global icons such as Killswitch Engage, As I Lay Dying, While She Sleeps, and Parkway Drive.

The moment Morning Waits became most widely known was the release of their single ""The Lasting Memories"" — an incredibly heartfelt yet powerful ballad. The single caught the attention of director Quoc Trung, executive director of Rockstorm 2014 – Hao Khi Dong A, making Morning Waits the only metal band to appear across three consecutive Rockstorm shows in Hai Phong, Hue, and Hanoi. They were also a prominent name at HardCore United 2015 and 2016, an event gathering metalcore and experimental bands from across the country and the region's finest acts.

After a period of absence focused on recording and completing their debut album, the band released ""The Travelers"" through DEV record label in 2019 to widespread praise from the community. The album chronicles the personal battles of each member as they kept pursuing their passion, capturing milestones in each individual's life throughout their years together. That same year they performed at director Tran Trung Linh's Rock'n'Share show alongside Vietnam's top rock names.

Morning Waits' current lineup:
• Nguyen Trung Anh
• Nguyen Quang
• Hoang Viet Anh
• Tran Bao Lam
• Ha Dang Duc

If you wonder why you should listen to Morning Waits, it's the raw passion, the bright singalong melodies riding fast and melodic guitar riffs, the relentless drive of drums and bass — all blending into music that makes it impossible to stand still.",
            HeroUrl = "/assets/bands/morningwait/hero.jpg",
            LogoUrl = "/assets/bands/morningwait/logo.png",
            IsFeaturedOnHome = true,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 2
        },
        #endregion
        #region Band 3 — Amongst The Fallen
        new Band
        {
            Id = 3,
            Name = "Amongst The Fallen",
            Bio = @"Amongst The Fallen thể hiện một tinh thần thép, ý chí kiên cường cộc cằn đặc trưng của dòng nhạc Hardcore. Tinh thần “Stand up for the broken to fight and rise again” thể hiện rõ thái độ không bỏ cuộc, không cúi đầu, ko chịu khuất phục.
 
Amongst The Fallen giai điệu cọc cằn đến từ Thủ Đô được thành lập vào giữa năm 2023 với định hướng theo đuổi dòng nhạc beatdown hardcore. Ban nhạc đã ra mắt EP đầu tay mang tên “Home Cook Hardcore” vào ngày 15/1/2024. EP mang đậm những nét Oldschool Hardcore và âm hưởng đã được band nhạc đúc kết từ Knockes Loose trú danh. Với nhịp Blast Beats dồn dập, câu riff nặng đô và giọng scream sặc mùi Long Biên sẽ mang lại năng lượng tích cực mạnh  mẽ tới mọi người.

“Nhập môn” Amongst The Fallen với EP “HOME COOK HARD CORE”:
• DEAD WEIGHT với những câu rif nặng nề nhằm sốc lại tinh thần bản thân nhằm vượt qua nghịch cảnh.
• 6 FEET là lời cảnh báo tới những kẻ ngáng đường, những lời đàm tiếu dè bỉu không để lọt tai.
• WORTTHLESs là lời tự nhắn nhủ bản thân, lời tự sự, tự trách và vực dậy từ thất bại.
• AMONGST THE FALLEN với lời kêu gọi, lời thúc giục con người đứng lên chiến đấu, không gục ngã, không bỏ cuộc và.

 Đây cũng là lần thứ hai band nhạc trẻ xuất hiện tại Sài Thành với hy vọng có thể mang đến năng lượng hừng hực của sức trẻ Thủ Đô. Điều gì sẽ xảy ra tiếp theo phụ thuộc vào sự xuất hiện của các bạn! 
",
            BioEn = @"Amongst The Fallen represents the rough, resilient spirit of hardcore music. Their motto, ""Stand up for the broken to fight and rise again,"" reflects an attitude of never giving up and never bowing down.

The Hanoi-based band was formed in mid-2023 with a beatdown hardcore direction. They released their debut EP ""Home Cook Hardcore"" on January 15, 2024, inspired by old-school hardcore and modern heavy influences.

Their songs carry raw energy and clear messages:
• DEAD WEIGHT: reset your spirit to overcome adversity.
• 6 FEET: a warning to obstacles and cynical voices.
• WORTTHLESs: self-reflection, accountability, and rising from failure.
• AMONGST THE FALLEN: a call to stand up, fight, and never quit.

This is their second time performing in Saigon, bringing the fiery energy of Hanoi youth to the stage.",
            HeroUrl = "/assets/bands/amongstthefallen/hero.jpg",
            LogoUrl = "/assets/bands/amongstthefallen/logo.png",
            IsFeaturedOnHome = false,
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 3
        },
        #endregion
        #region Band 4 — Black Industry
        new Band
        {
            Id = 4,
            Name = "Black Industry",
            Bio = @"BLACK INDUSTRY được thành lập vào cuối năm 2023, với chỉ 2 thành viên founder, Andie Christ và Jon Apollyon với âm thanh máy móc đanh đét và nặng nề của dòng nhạc Industrial Metal. 

Qua thời gian phát triển và ra mắt ca khúc single đầu tay trong EP 'Industry Whore': 'Braindead' với sự ủng hộ nhiệt tình từ người nghe, 2 thành viên quyết định lập band nhạc chính thức tên là Black Industry. 

Họ đã chiêu mộ các vị trí còn thiếu trong band để mang lại các màn trình diễn live chất lượng nhất, từ đó kết nạp thêm: Bush (lead guitar), Cbosi (bassist), Richfield (Percussionist) và Brian (Drummer). 

Đầu năm 2026, BLACK INDUSTRY chia tay 2 thành viên Jon Apollyon (guitar chính của band) và Cbosi (bassist của band). Vì tính chất công việc và học tập nên 2 thành viên này không thể tiếp tục đồng hành cùng band.

Ở thời điểm hiện tại band đã có có 1 vài thành viên mới để thay thế cho 2 vị trí còn sót đó là Luna (hát chính) và Hoàng Phương (guitar session) và X (bassist).

Black Industry  phát album đầu tay có tên 'My Tears Run BLACK’ vào giữa năm 2025. Album là một bản tuyên ngôn đậm chất Industrial Metal, album kết hợp nhiều yếu tố thể loại như Electro, Synthcore, Techno, Nu Metal, Alternative Metal, Symphonic Metal và Metalcore - tạo nên một không gian âm nhạc mạnh mẽ và độc đáo. 'My Tears Run BLACK' là hành trình khai phá xung đột nội tâm, tình yêu, xã hội và những mối quan hệ. Hiện có thể nghe album trên các nền tảng trực tuyến.

THÀNH VIÊN HIỆN TẠI:
• Trung Kiên (Andie Christ) - vocalist, bassist
• Thái Huân (Brian) - drummer
• Ngọc Ánh (Luna) - vocalist
• Tiến Phát (Richfield) - percussionist
• Duy Anh (Bush) - guitarist
• Hoàng Phương - guitarist session

THỂ LOẠI: Alternative Metal, Industrial Metal, Nu Metal

NHỮNG SẢN PHẨM ĐÃ RA MẮT:
Album “My Tears Run Black”: https://youtu.be/No9uWqxz2D0?si=GfPbA4_LyJSo8EZv
The Letter MV:https://youtu.be/fZ_rHDKggv8?si=XKVbxOVI7-3nYiZi
Heartless MV:https://youtu.be/F9ld3NGyjXA?si=SHId5Lry786wmaR7
Braindead MV:https://youtu.be/P_SCJHSgaNw?si=9lmeSKof5bsswmVH",
            BioEn = @"BLACK INDUSTRY was founded in late 2023 by Andie Christ and Jon Apollyon, pursuing a sharp, heavy Industrial Metal sound.

After early positive response to their first single ""Braindead"" from the EP ""Industry Whore,"" they formalized the lineup for full live performances. In early 2026, two members left due to work/study reasons, and new members joined.

The band released their debut album ""My Tears Run BLACK"" in mid-2025, blending Industrial Metal with Electro, Synthcore, Techno, Nu Metal, Alternative Metal, Symphonic Metal, and Metalcore. The album explores inner conflict, love, society, and relationships.

Current members:
• Trung Kien (Andie Christ) - vocalist, bassist
• Thai Huan (Brian) - drummer
• Ngoc Anh (Luna) - vocalist
• Tien Phat (Richfield) - percussionist
• Duy Anh (Bush) - guitarist
• Hoang Phuong - session guitarist",
            HeroUrl = "/assets/bands/blackindustry/hero.JPG",
            LogoUrl = "/assets/bands/blackindustry/logo.png",
            IsFeaturedOnHome = false,
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 4
        },
        #endregion
        #region Band 5 — Cút Lộn
        new Band
        {
            Id = 5,
            Name = "Cút Lộn",
            Bio = @"Cút Lộn là ban nhạc hardcore Việt Nam được thành lập năm 2018. Ban đầu nhóm theo phong cách crossover thrash với album đầu tay Xào Ke (đầu 2019), sau đó phát hành thêm 3 đĩa đơn trong giai đoạn 2020-2023.

Đầu năm 2024, ban nhạc thay đổi vocalist và chuyển hướng âm nhạc sang nặng hơn, tối hơn và hỗn loạn hơn. Với đội hình mới, ban nhạc phát hành album phòng thu thứ hai “Bắc Thảo” vào tháng 8/2024 và album thứ ba “Dzữa” vào tháng 10/2025.

Cút Lộn được biết đến với sân khấu bùng nổ và ca từ mỉa mai nhưng giàu chất thơ bằng tiếng Việt. Gần như mọi bài trong giai đoạn crossover thrash đều có MV trên YouTube, và ban nhạc cũng phát hành thêm hai MV cho các ca khúc mới từ Bắc Thảo.

Ban nhạc đã diễn nhiều show trên khắp Việt Nam và tại Hàn Quốc, Thái Lan, Malaysia, Philippines, Campuchia. Một số cột mốc nổi bật gồm Rock Alarm Festival 2024 (Bangkok) và Baybeats Festival 2025 (Singapore).

Tour tháng 11-12/2025 của nhóm cũng có các chặng tại Trung Quốc, Malaysia và Philippines.

Đội hình:
• Vui Qá: Vocal
• Quang Sọt: Guitar
• Nguyên Lê: Bass
• Sergey: Drums",
            BioEn = @"Cút Lộn is a Vietnamese hardcore band formed in 2018. They started as a crossover thrash band and released their debut album Xào Ke in early 2019, followed by three singles during 2020-2023.

In early 2024, the band changed vocalist and shifted to a heavier, darker, and more chaotic direction. With this lineup, they released their second studio album ""Bắc Thảo"" in August 2024, then their third album ""Dzữa"" in October 2025.

Cút Lộn is known for high-energy live performances and sarcastic yet poetic Vietnamese lyrics. Almost every song from their crossover thrash era has an MV on their YouTube channel, and they also released two MVs for newer songs from Bắc Thảo.

The band has performed across Vietnam and in South Korea, Thailand, Malaysia, the Philippines, and Cambodia. Notable appearances include Rock Alarm Festival 2024 in Bangkok and Baybeats Festival 2025 in Singapore.

Their November-December 2025 tour also includes dates in China, Malaysia, and the Philippines.

Lineup:
• Vui Qá: Vocal
• Quang Sọt: Guitar
• Nguyên Lê: Bass
• Sergey: Drums",
            HeroUrl = "/assets/bands/cutlon/hero.jpg",
            LogoUrl = "/assets/bands/cutlon/logo.png",
            IsFeaturedOnHome = true,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 5
        },
        #endregion
        #region Band 6 — Die So Far
        new Band
        {
            Id = 6,
            Name = "Die So Far",
            Bio = @"Die So Far là một dự án âm nhạc đến từ Nha Trang, được thành lập vào giữa tháng 11 năm 2024 bởi Kiệt và Thiên – hai trong 4 đồng sáng lập của cộng đồng Nha Trang Heavy Souls. Ban nhạc theo đuổi phong cách Hardcore, với âm thanh thô ráp, dữ dội và tràn đầy năng lượng sân khấu.
Die So Far được hình thành như một cách để cả hai tiếp tục phát triển những ý tưởng còn dang dở từ ban nhạc cũ, đồng thời thể hiện tiếng nói cá nhân và tinh thần phản kháng của riêng mình.
Ngày 15 tháng 7 năm 2025, Die So Far chính thức phát hành EP đầu tay mang tên “We Are Good Kids”, đánh dấu cột mốc đầu tiên trong hành trình sáng tạo độc lập của nhóm.
Sau khi Kiệt và Thiên quyết định vào TP.HCM để học tập, cả hai thống nhất dựng đội hình full band gồm:
• Vô Thức – Bass
• Đồng Thanh Sơn – Drums
• Phan Hoàng Phương (Ben) – Guitar

Từ khi thành lập, Die So Far đã tham gia biểu diễn tại nhiều sự kiện tiêu biểu như:
• Huynh Đệ In Arms (TP.HCM)
• Reborn (Nha Trang)
• We Are Good Kids – EP Listening Show (Nha Trang)
• Summer Madness (Nha Trang) 
",
            BioEn = @"Die So Far is a music project from Nha Trang, founded in mid-November 2024 by Kiet and Thien—two of the four co-founders of the Nha Trang Heavy Souls community. The band follows a hardcore direction with a raw, aggressive sound and high live energy.

Die So Far was created to continue ideas left unfinished in their former band while expressing their personal voice and spirit of resistance.

On July 15, 2025, Die So Far released their debut EP ""We Are Good Kids"", marking the first milestone in the group’s independent creative journey.

After Kiet and Thien moved to Ho Chi Minh City for study, they assembled a full lineup:
• Vo Thuc - Bass
• Dong Thanh Son - Drums
• Phan Hoang Phuong (Ben) - Guitar

Since formation, Die So Far has performed at:
• Huynh Đệ In Arms (HCMC)
• Reborn (Nha Trang)
• We Are Good Kids - EP Listening Show (Nha Trang)
• Summer Madness (Nha Trang)",
            HeroUrl = "/assets/bands/diesofar/hero.JPG",
            LogoUrl = "/assets/bands/diesofar/logo.png",
            IsFeaturedOnHome = false,
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 6
        },
        #endregion
        #region Band 7 — Elbow Drop
        new Band
        {
            Id = 7,
            Name = "Elbow Drop",
            Bio = @"ElbowDrop là băng nhạc Hardcore đến từ TP. Hồ Chí Minh, thành lập đầu năm 2022. Sau nhiều lần thay đổi thành viên, lineup hiện tại gồm Quân (vocal), Quang Sọt (guitar), Aki (bass) và Sergey (drums). 

Ra mắt single “Kind Words” và “Thick Skin” năm 2022, sau đó phát hành EP đầu tay ElbowDrop (2023) với bốn ca khúc: Sweet Justice, Solved By Violence, Built Different, Zero Tolerance. Những sản phẩm âm nhạc mới đang được nấu và dự kiến ra mắt vào cuối năm 2025. ElbowDrop được biết tới bởi biểu diễn ở các show Hardcore tại Sài Gòn, Hà Nội và Kuala Lumpur.

Discography: Kind Words (2022), Thick Skin (2022), ElbowDrop EP (2023)
Các thành viên:
• Nguyễn Hoàng Quân 
• Sergey Bochenkov
• Nguyễn Duy Quang
• Phạm Xuân Hoàng
",
            BioEn = @"Elbow Drop is a hardcore band from Ho Chi Minh City, formed in early 2022. After several lineup changes, the current members are Quan (vocal), Quang Sot (guitar), Aki (bass), and Sergey (drums).

They released the singles ""Kind Words"" and ""Thick Skin"" in 2022, then their self-titled EP ElbowDrop (2023) with four tracks: Sweet Justice, Solved By Violence, Built Different, and Zero Tolerance. New releases are in progress for late 2025.

Discography: Kind Words (2022), Thick Skin (2022), ElbowDrop EP (2023).",
            HeroUrl = "/assets/bands/elbowdrop/hero.jpg",
            LogoUrl = "/assets/bands/elbowdrop/logo1.png",
            IsFeaturedOnHome = true,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 7
        },
        #endregion
        #region Band 8 — Empathize
        new Band
        {
            Id = 8,
            Name = "Empathize",
            Bio = @"Vào cuối tháng 7/2023, sau khi gặp nhau trong một quán cà phê ở Sài Gòn, Kiên và Hiếu quyết định thành lập band, theo đuổi dòng nhạc hardcore. Quá trình tìm kiếm những mảnh ghép còn thiếu để hoàn thiện đội hình đã đưa Bảo và Mai Phương đến với band. 
Tuy nhiên, con đường âm nhạc không hề bằng phẳng. Trải qua nhiều biến động về thành viên, Empathize đã dần định hình với đội hình hiện tại gồm 6 thành viên:
• Bao (Vocalist)
• Anh Khoi (Vocalist)
• Phan Hoang Phuong (Guitarist)
• Kien Bui (Guitarist)
• Mai Phuong (Drummer)
• Duc Anh (Bassist)

Với âm hưởng metallic hardcore đầy mạnh mẽ, ban nhạc trẻ Empathize đã khẳng định dấu ấn của mình trong hơn một năm qua. Ba show local đã chứng tỏ được sự nhiệt huyết và tài năng của các thành viên. Single đầu tay ""Deluded"" đã nhanh chóng tạo được tiếng vang, đánh dấu một bước khởi đầu đầy hứa hẹn.
Mang trong mình nhiệt huyết của tuổi trẻ, Empathize không chỉ đơn thuần tạo ra âm nhạc để giải trí mà còn muốn gửi gắm những thông điệp ý nghĩa. Band lên án mạnh mẽ những điều xấu xa, bất công trong xã hội, đồng thời khơi dậy tinh thần đấu tranh cho công lý. Tuy nhiên, thông điệp của Empathize không dừng lại ở sự thù ghét. Thay vào đó, band muốn truyền tải một thông điệp tích cực hơn, đó là sự thấu cảm và luôn luôn tiến về phía trước vì một tương lai tốt đẹp hơn.
",
            BioEn = @"In late July 2023, after meeting in a café in Saigon, Kien and Hieu decided to form a hardcore band. The search for missing pieces brought Bao and Mai Phuong into the project.

The journey was not smooth. After multiple lineup changes, Empathize is now a six-member band:
• Bao (Vocalist)
• Anh Khoi (Vocalist)
• Phan Hoang Phuong (Guitarist)
• Kien Bui (Guitarist)
• Mai Phuong (Drummer)
• Duc Anh (Bassist)

With a powerful metallic hardcore sound, Empathize has built momentum over the past year. Three local shows proved the members’ passion and skill, and their debut single ""Deluded"" quickly gained attention.

Empathize creates music not only for entertainment, but to deliver meaningful messages. The band strongly condemns injustice and social wrongs while encouraging struggle for fairness. More importantly, their message goes beyond hatred—toward empathy and moving forward for a better future.",
            HeroUrl = "/assets/bands/empathize/hero.jpg",
            LogoUrl = "/assets/bands/empathize/logo.png",
            IsFeaturedOnHome = false,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 8
        },
        #endregion
        #region Band 9 — Hypertension
        new Band
        {
            Id = 9,
            Name = "Hypertension",
            Bio = @"Hypertension – Khi Technical Death Metal gặp gỡ vẻ đẹp tàn khốc của Cổ điển
Hypertension (Tăng Huyết Áp) – Ngay từ cái tên, ban nhạc đã khẳng định một nguồn năng lượng bùng nổ, một áp lực không thể kìm hãm, sẵn sàng phá vỡ mọi giới hạn. Đến từ Việt Nam, Hypertension không chỉ chơi nhạc, họ đang kiến tạo một thế giới âm thanh nơi sự phức tạp kỹ thuật của Technical Death Metal và tư duy mở của Progressive Metal hòa quyện cùng sự tráng lệ, kịch tính của âm nhạc Cổ điển.
Đây là âm nhạc của những cơn ""tăng huyết áp"" nghệ thuật. Mỗi ca khúc là một kiến trúc âm thanh đồ sộ, được xây dựng từ những riff guitar phức tạp, đối xứng như những bản giao hưởng điên loạn; nhịp trống hung bạo nhưng đầy tính toán, như nhịp đập của một trái tim cơ khí; và giọng hát linh hoạt, biến hóa từ tiếng gầm gừ địa ngục đến những giai điệu nội tâm giằng xé.

Những Kiến Trúc Sư Của ""Áp Lực"":
• Hùng Đàm (Guitarist): Vị nhạc trưởng của những riff guitar sắc bén, hòa quyện tư duy Progressive Metal hiện đại với sự đối lập và cân bằng của âm nhạc Cổ điển.
• Hoài Thanh (Vocal): Sở hữu giọng hát có thể ""kể chuyện,"" từ sự dữ dội tột cùng đến những nốt nhạc phức tạp, lột tả sâu sắc những mâu thuẫn nội tâm và áp lực của xã hội hiện đại.
• Trúc Lộc (Bassist): Trái tim của những dòng chảy bass đầy kỹ thuật, được rèn giũa qua thời gian, tạo nền móng vững chắc cho toàn bộ kiến trúc âm nhạc.
• Cường Bùi (Drummer): Động cơ không ngừng nghỉ của ban nhạc, sở hữu nhịp trống chính xác tuyệt đối, là sự pha trộn giữa Death Metal hung bạo và sự phức tạp của Progressive.

Hành Trình Chinh Phục:
Với Hypertension, âm nhạc không chỉ là một trải nghiệm nghe, mà là một trải nghiệm giác quan tổng thể. Ban nhạc không ngừng thử nghiệm, từ việc đưa những yếu tố folk Việt Nam vào một không gian Technical Death Metal, đến việc tạo ra những bản thu chất lượng cao và những buổi biểu diễn trực tiếp rực lửa. Với tầm nhìn đưa Technical Metal Việt Nam ra thế giới, Hypertension biến ""áp lực"" thành sức mạnh, biến nhạc nặng thành một loại nghệ thuật đỉnh cao.
Hãy chuẩn bị tinh thần cho một cơn ""Tăng Huyết Áp"" thực sự!
",
            BioEn = @"Hypertension - When Technical Death Metal meets the brutal beauty of Classical music.
Hypertension (""High Blood Pressure"") - right from the name, the band declares explosive energy and unstoppable pressure, ready to break all limits. From Vietnam, Hypertension does more than play music: they build a sonic world where the technical complexity of Technical Death Metal and the open mindset of Progressive Metal merge with the grandeur and dramatic intensity of Classical music.
This is music of artistic ""hypertensive"" surges. Each song is a massive sound architecture, built from complex and symmetrical guitar riffs like deranged symphonies; violent yet calculated drumming like the pulse of a mechanical heart; and flexible vocals that shift from infernal growls to torn, introspective melodies.

The Architects of ""Pressure"":
• Hung Dam (Guitarist): the conductor of sharp guitar riffs, blending modern Progressive Metal thinking with the contrast and balance of Classical music.
• Hoai Thanh (Vocal): a storytelling voice that moves from extreme ferocity to intricate melodic lines, deeply expressing inner conflict and the pressure of modern society.
• Truc Loc (Bassist): the heart of technical bass flows, forged over time, creating a solid foundation for the whole musical structure.
• Cuong Bui (Drummer): the band’s relentless engine, with highly precise drumming that combines Death Metal aggression and Progressive complexity.

The Journey of Conquest:
For Hypertension, music is not only a listening experience, but a full sensory experience. The band keeps experimenting, from bringing Vietnamese folk elements into Technical Death Metal space to creating high-quality recordings and blazing live performances. With a vision of taking Vietnamese Technical Metal to the world, Hypertension turns ""pressure"" into strength and heavy music into high art.
Prepare yourself for a real ""Hypertension"" surge!",
            HeroUrl = "/assets/bands/hypertension/hero.png",
            LogoUrl = "/assets/bands/hypertension/logo.png",
            IsFeaturedOnHome = true,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 9
        },
        #endregion
        #region Band 10 — Kinh
        new Band
        {
            Id = 10,
            Name = "Kinh",
            Bio = @"KINH được thành lập vào giữa năm 2023 tại Sài Gòn bởi Huy Goodman, Huyên Blaster và Siêu Dê Cụ với mong muốn sáng tạo và trình diễn extreme music.

Nhóm chơi death metal chịu ảnh hưởng từ hardcore, grindcore và cả một chút hip hop. KINH tin rằng âm nhạc không có giới hạn, nên luôn để trí tưởng tượng vượt qua ranh giới khi tạo ra nhạc nặng.

Thể loại: Death Metal
Địa điểm: Sài Gòn, Việt Nam
Youtube: @Kinh_SaigonDeathMetal
Facebook: www.facebook.com/KinhSaigonDeathMetal/
Instagram: www.instagram.com/kinh_saigondeathmetal/
Email: Kinhextreme.official@gmail.com

Đội hình:
• Siêu DÊ cụ - No Treble/Voice
• Huy Goodman - Guitar
• Huyên Blaster - Drum

NỘ - Single 2024
Phát hành: 13/05/2024

VÔ - Single 2025
Phát hành: 08/08/2025
",
            BioEn = @"KINH was formed in mid-2023 in Saigon, Vietnam, by Huy Goodman, Huyen Blaster, and Sieu De Cu with the intention of creating and playing extreme music. We play death metal influenced by genres such as hardcore, grindcore, and even a touch of hip hop. We believe there are no limitations in music and let our imagination go beyond boundaries when creating heavy music.

Genre: Death Metal
Location: Saigon, Vietnam
Youtube: @Kinh_SaigonDeathMetal
Facebook: www.facebook.com/KinhSaigonDeathMetal/
Instagram: www.instagram.com/kinh_saigondeathmetal/
Email: Kinhextreme.official@gmail.com

Lineup:
• Sieu De Cu - No Treble/Voice
• Huy Goodman - Guitar
• Huyen Blaster - Drum

NỘ - Single 2024
Releases: May 13th, 2024

VÔ - Single 2025
Releases: Aug 8th, 2025
",
            HeroUrl = "/assets/bands/kinh/hero.jpg",
            LogoUrl = "/assets/bands/kinh/logo.png",
            IsFeaturedOnHome = true,
            IsSecret = true,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 10
        },
        #endregion
        #region Band 11 — Nhao
        new Band
        {
            Id = 11,
            Name = "Nhạo",
            Bio = @"NHẠO là ban nhạc rock Việt Nam được thành lập vào ngày 20/09/2020 và hiện tại đang hoạt động ở Sài Gòn với 4 thành viên ban nhạc: 
• Trọng Nhân (Band Leader - Vocalist)
• Thanh Duy (Rhythm Guitarist)
• Quốc Bảo (Drummer)
• Bảo Long (Bassist) 

NHẠO trong Hán - Việt  樂 có nghĩa là “nhạc” và trong tiếng Nôm có nghĩa là “nhạo báng”, “chế nhạo”.  Âm nhạc của NHẠO mang sự ngông cuồng, kiêu ngạo của tuổi trẻ để nói lên góc nhìn của ban nhạc đối với những vấn đề khác nhau trong cuộc sống.

Luôn học hỏi và nâng cao bản thân từng ngày, NHẠO sẽ cố gắng phát triển để đạt được những ước mơ mà cả đám hướng tới. 
",
            BioEn = @"NHẠO is a Vietnamese rock band founded on 20/09/2020 and currently active in Saigon with four members:
• Trong Nhan (band leader - vocalist)
• Thanh Duy (rhythm guitarist)
• Quoc Bao (drummer)
• Bao Long (bassist)

The name “”NHẠO”” carries dual meanings: “”music”” in Sino-Vietnamese context and “”mockery/satire”” in vernacular usage. Their music channels youthful arrogance and candid perspectives on everyday social issues.

The band keeps learning and improving to pursue its long-term artistic goals.",
            HeroUrl = "/assets/bands/nhao/hero.jpeg",
            LogoUrl = "/assets/bands/nhao/logo.png",
            IsFeaturedOnHome = false,
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 11
        },
        #endregion
        #region Band 12 — Resurged Temptation
        new Band
        {
            Id = 12,
            Name = "Resurged Temptation",
            Bio = @"Resurged Temptation là một ban nhạc metalcore Việt Nam đến từ Sài Gòn, Việt Nam, được thành lập vào cuối năm 2022.

Các thành viên ban đầu tập hợp trong phòng tập nhạc vào cuối năm 2022 để cùng nhau sáng tác. 

Vào tháng 5 năm 2023, chính thức thông báo rằng Đinh Bảo Nam sẽ gia nhập ban nhạc với vai trò lead vocalist mới, qua đó hoàn thiện đội hình. 

Họ đã phát hành đĩa đơn đầu tay, ""Idiopathic"" vào ngày 31 tháng 10 năm 2023. 

Vào ngày 7 tháng 5 năm 2024, họ phát hành một đĩa đơn mới mang tên ""Break Their Rules."" 

Ngày 9 tháng 3 năm 2025 vừa qua band nhạc đã phát hành MV mới nhất ""The Power of Weakness"" và nhận được phản hồi khá tích cực

Hiện tại, ban nhạc đang tiếp tục thu âm các đĩa đơn mới song song với việc tập trung vào việc sáng tác các sản phẩm mới.

Chất liệu của Resurged Temptation là cảm hứng từ thể loại metalcore. Tuy nhiên, phong cách âm nhạc tổng thể của ban nhạc được các thành viên miêu tả là sự kết hợp giữa metalcore, melodic death và groove metal. Đặc trưng phong cách của họ bao gồm việc sử dụng các đoạn breakdowns, growling vocals, clean vocals và các đoạn riff hơi hướng groove.


Thành viên:
• Nguyễn Thành Các - guitar rhythm 
• Trần Hoàng Phúc - guitar lead 
• Đinh Bảo Nam - lead vocalist
• Trương Kim Trọng - bass, backing vocal
• Võ Phúc Hoan - drummer",
            BioEn = @"Resurged Temptation is a Vietnamese metalcore band from Saigon, founded in late 2022.

The early members gathered in rehearsal spaces in late 2022 to write together.

In May 2023, Đinh Bảo Nam officially joined as lead vocalist, completing the lineup.

They released their debut single ""Idiopathic"" on October 31, 2023.

On May 7, 2024, they released a new single titled ""Break Their Rules.""

On March 9, 2025, the band released their latest MV ""The Power of Weakness,"" which received positive feedback.

Currently, the band continues recording new singles while focusing on writing new material.

Resurged Temptation draws inspiration from metalcore, but the overall style is described by members as a blend of metalcore, melodic death, and groove metal. Their trademark elements include breakdowns, growling vocals, clean vocals, and groove-oriented riffing.

Members:
• Nguyen Thanh Cac - rhythm guitar
• Tran Hoang Phuc - lead guitar
• Dinh Bao Nam - lead vocalist
• Truong Kim Trong - bass, backing vocal
• Vo Phuc Hoan - drummer",
            HeroUrl = "/assets/bands/resurgedtemptation/hero.jpg",
            LogoUrl = "/assets/bands/resurgedtemptation/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 12,
            IsFeaturedOnHome = true,
        },
        #endregion
        #region Band 13 — Sóng Âm
        new Band
        {
            Id = 13,
            Name = "Sóng Âm",
            Bio = @"Từ những ngày hè của năm 2022, 5 cậu sinh viên với niềm tin mãnh liệt vào âm nhạc của mình có cơ duyên được kết nối với nhau qua những buổi giao lưu câu lạc bộ, từ những buổi gặp mặt cà phê. Từ chính sự đam mê và nhiệt huyết ấy, ban nhạc được chính thức thành lập bao gồm 5 thành viên: 
• Nguyen Du Giang Niekdam (Vocal)
• Tran Ngoc Bao Tin (Guitarist)
• Luu Minh Tri (Guitarist)
• Lam Quoc Bao Long (Bassist)
• Lam Nhat Duc (Drummer)

 Ngày 12.12.2022 Sóng Âm chính thức ra mắt Single đầu tiên mang tên ‘’Ngày Mai’’ mang được nhiều hiệu ứng tích cực đến cho người nghe, thể hiện được tinh thần tuổi trẻ và sự nhiệt huyết luôn rực cháy, ngoài ra Sóng Âm đã có thêm cho mình 2 sản phẩm âm nhạc là “Kể Cho Tôi và “Đi”, Sóng Âm luôn có một mục tiêu, đó là âm nhạc không chỉ để nghe, mà còn là kể chuyện, là sự đồng cảm với những câu chuyện của chính khán giả. Chính vì lẽ đó, Sóng Âm luôn cố gắng và nỗ lực hơn mỗi ngày để ra đời các sản phẩm âm nhạc chỉn chu nhất có thể. Và 21/3 vừa rồi, “We are the one” ra đời đánh dấu sự trở lại của ban nhạc.
Dù có nhiều biến động đối với thành viên, đôi lúc cũng khó khăn như bao ban nhạc khác, Sóng Âm vẫn luôn giữ cho mình niềm tin về những gì mình đang làm và đang theo đuổi với khát khao lớn nhất đó chính là “chạm” đến khán giả bằng cả trái tim của ban nhạc.
",
            BioEn = @"In the summer of 2022, five students connected through club meetups and casual coffee gatherings, united by strong faith in music. From that shared passion and energy, the band was officially formed with five members:
• Nguyễn Du Giang Niêkdam (Vocal)
• Trần Ngọc Bảo Tín (Guitarist)
• Lưu Minh Trí (Guitarist)
• Lâm Quốc Bảo Long (Bassist)
• Lâm Nhật Đức (Drummer)

On 12/12/2022, Sóng Âm released their first single ""Ngày Mai,"" receiving positive response and expressing youth spirit and relentless enthusiasm. The band later released “”Kể Cho Tôi”” and “”Đi.””

Sóng Âm’s mission is that music is not only for listening—it is storytelling and empathy with listeners’ own life stories. On 21/3, ""We Are The One"" marked the band’s return.

Despite lineup changes and difficulties like many bands, Sóng Âm keeps faith in their path and pursues their biggest goal: to truly touch audiences with heart.",
            HeroUrl = "/assets/bands/songam/hero.png",
            LogoUrl = "/assets/bands/songam/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 13
        },
        #endregion
        #region Band 14 — Thánh Dực
        new Band
        {
            Id = 14,
            Name = "Thánh Dực",
            Bio = @"Chiến tranh để lại những vết sẹo không lời – nhưng với Thánh Dực, những vết sẹo đó biết gầm lên bằng âm thanh.

Được đặt tên theo đội quân “Thánh Dực Dũng Nghĩa” huyền thoại của triều Trần, ban nhạc là một bản giao hưởng hiện đại của những trận đánh huyết lệ đã tạo nên linh hồn Việt Nam. 

Được thành lập vào năm 2019 với cảm hứng từ old-school death metal và các sự kiện lịch sử, Thánh Dực là ban nhạc gồm 6 thành viên đến từ Sài Gòn, mang trong mình khát vọng kể lại lịch sử Việt Nam bằng ngôn ngữ âm nhạc mới.

Thông qua thể loại melodic death metal – mạnh mẽ, dữ dội nhưng đầy cảm xúc – chúng tôi tái hiện những trang sử hào hùng và bi tráng của dân tộc Việt Nam, từ đó lan tỏa giá trị văn hoá Việt đến khán giả Việt & quốc tế.

Lấy cảm hứng từ những trận đánh oanh liệt chống ngoại xâm trong lịch sử Việt Nam, nhóm không đơn thuần mô tả chiến tranh, mà chuyển thể chủ đề này thành trải nghiệm âm thanh đậm chất metal rock.

Âm nhạc của Thánh Dực là cây cầu nối giữa truyền thống và hiện đại, giữa ký ức và tương lai – nơi người trẻ có thể cảm nhận & đối thoại với quá khứ để kiến tạo một xã hội tương lai hiện đại nhưng đầy bản sắc và thấu cảm.

Trong mỗi phần trình diễn, khán giả không chỉ nghe, mà còn trải nghiệm lịch sử & ký ức dân tộc thông qua một ngôn ngữ âm nhạc mới kết hợp cùng nghệ thuật trình chiếu hình ảnh.

Thành Viên:
• Thanh Phạm - Vocal
• Cường Bùi - Drummer
• Hào Phạm - Bassist
• Phát Phan - Guitarist
• Khôi Phạm - Guitarist
• Bảo Vũ - Keyboard
",
            BioEn = @"Thánh Dực transforms the scars of war into melodic death metal storytelling.

Inspired by the legendary “”Thánh Dực Dũng Nghĩa”” force of the Trần dynasty, the band was founded in 2019 in Saigon to retell Vietnamese history through modern heavy music.

Their work combines emotional melodic death metal with historical themes, visual presentation, and cultural identity, creating a bridge between tradition and contemporary expression.

Members:
• Thanh Pham - vocal
• Cuong Bui - drums
• Hao Pham - bass
• Phat Phan - guitar
• Khoi Pham - guitar
• Bao Vu - keyboard",
            HeroUrl = "/assets/bands/thanhduc/hero.jfif",
            LogoUrl = "/assets/bands/thanhduc/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 14
        },
        #endregion
        #region Band 15 — Under Pressure
        new Band
        {
            Id = 15,
            Name = "Under Pressure",
            Bio = @"Được thành lập vào ngày 01.06.2024 tại Sài Gòn, Việt Nam, là một ban nhạc theo đuổi dòng nhạc Metallic Hardcore bao gồm những cá nhân trẻ tuổi, đầy nhiệt huyết. Hiện tại, đội hình gốc của ban nhạc bao gồm: Đạt lê (Drummer), Jackk Nguyễn (Lead Guitar) và Hiếu Minh (Bassist). Gần đây, ban nhạc đã bổ sung thêm vị trí Rhythm Guitar là Khoa (aka Bác Trịnh) và Vocalist là Ares

Vào ngày 31.12.2025 vừa rồi, band đã cho ra mắt EP “Omnipotent”. Qua EP lần này, Under Pressure chúng tôi muốn truyền tải những thông điệp, cảm xúc và tinh thần mà hiếm khi được nhắc đến trong những dòng nhạc đại chúng. Lấy cảm hứng từ các cuộc chiến tranh đã diễn ra xuyên suốt hằng thế kỷ, từng bài trong EP mang những góc nhìn của các bên trong thời chiến. Bài #3 - “Dethroned” là những lời kêu gọi, tinh thần của quân kháng chiến chống lại cường quyền tàn bạo. Bài #2 - “Seraph” mang góc nhìn của những người lính chiến đấu, sẵn sàng chết vì tổ quốc khi đất nước họ bị xâm lược bởi các thế lực thù địch.

Đúng với cái tên “Omnipotent”, ban nhạc muốn nhấn mạnh rằng đằng sau những âm thanh ồn ào và bạo lực ấy là lời nhắn nhủ về một phong cách sống độc lập, có chính kiến, dám đứng lên và hành động, thực hiện những gì bản thân mình tin tưởng.
",
            BioEn = @"Formed on 01/06/2024 in Saigon, Vietnam, Under Pressure is a Metallic Hardcore band made up of young, passionate members. The original lineup includes Dat Le (drummer), Jackk Nguyen (lead guitar), and Hieu Minh (bassist). Recently, the band added Khoa (aka Bac Trinh) on rhythm guitar and Ares on vocals.

On 31/12/2025, the band released the EP ""Omnipotent."" Through this release, Under Pressure aims to express themes, emotions, and spirit rarely discussed in mainstream music. Inspired by wars across centuries, each track presents perspectives from different sides in wartime:
• Track #3 - ""Dethroned"": a call and spirit of resistance against oppressive power.
• Track #2 - ""Seraph"": the perspective of soldiers willing to die for their homeland when invaded.

True to the title ""Omnipotent,"" the band emphasizes that behind noisy and violent sounds is a message about independent living, strong personal values, and the courage to stand up and act on what you believe.",
            HeroUrl = "/assets/bands/underpressure/hero.jpg",
            LogoUrl = "/assets/bands/underpressure/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 15
        },
        #endregion
        #region Band 16 — 1818
        new Band
        {
            Id = 16,
            Name = "1818",
            Bio = "",
            HeroUrl = "/assets/bands/1818/hero.png",
            LogoUrl = null,
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 16
        },
        #endregion
        #region Band 17 — 7 Uppercuts
        new Band
        {
            Id = 17,
            Name = "7 Uppercuts",
            Bio = "",
            HeroUrl = "/assets/bands/7uppercuts/hero.png",
            LogoUrl = null,
            IsSecret = true,
            IsFeaturedOnHome = true,
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 17
        },
        #endregion
        #region Band 18 — 9xacly
        new Band
        {
            Id = 18,
            Name = "9xacly",
            Bio = "Thành lập vào 2022, 9xacly là band nhạc hardcore punk gồm 5 thành viên:\n\n• Tòng (vocal)\n• Thái (drums)\n• Hardy (guitar)\n• Chuột Sấm Xét (guitar)\n• Vui Qá (bass)\n\nVà họ cùng nhau chơi rất vui.",
            BioEn = "Formed in 2022, 9xacly is a 5-piece hardcore punk band consisting of:\n\n• Tòng (vocals)\n• Thái (drums)\n• Hardy (guitar)\n• Chuột Sấm Xét (guitar)\n• Vui Qá (bass)\n\nAnd they have a great time playing together.",
            HeroUrl = "/assets/bands/9xacly/hero.jpeg",
            LogoUrl = "/assets/bands/9xacly/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 18
        },
        #endregion
        #region Band 19 — Diarsia
        new Band
        {
            Id = 19,
            Name = "Diarsia",
            Bio = @"DIARSIA là ban nhạc Hybrid Deathcore được thành lập vào cuối năm 2019 và đầu năm 2020. Dòng nhạc được định hình bởi màu sắc tối, những câu breakdown bất ngờ pha lẫn cảm hứng từ nhiều thể loại khác nhau: Metalcore, Djent, Thall, Slam. Đồng thời, yếu tố Hybrid cũng là điểm nổi bật của ban nhạc khi cái gai góc thuần túy của dòng nhạc đang theo đuổi được kết hợp cùng các chất liệu độc đáo, từ nhạc cụ dân tộc cho đến EDM, Phonk, Trap,… 
EX-MEMBER:
• Ty Phạm – bass (10/2020 - 06/2023) 
• Nguyễn Quốc Việt – guitar (02/2020 - 09/2020) 
• Nguyễn Shien – guitar (02/2020 - 05/2020) 
• Nguyễn Thảo – bass (01/2020 - 09/2020) 
• Tú Lỳ – drum (02/2020 - 04/2023)
• Ling Nguyễn – guitar (01/2021 - 01/2025)

CURRENT MEMBERS:
 • Khang Nguyễn – vocals, frontman (01/2020 - hiện tại)
 • Tuấn Kiệt – drum (09/2023 - hiện tại)
 • Xuân Khôi – guitar  (06/2020 - hiện tại)
 • Đăng Khoa– bass (01/2025 - hiện tại)

CÁC TÁC PHẨM ĐÃ ĐƯỢC RA MẮT:
1. Salvation – 10.12.2020
2. Purge (ft. Huy Gái from I’m Not Sure) – 25.07.2021
3. The Plague – 30.09.2022
4. Bloodmoon – 15.12.2022
5. Berzerker (ft. Ryu Miura from DIVINITIST JP) – 21.07.2023
6. Self Destruction (ft. Huỳnh Huy from District105) – 23.11.2023
7. Armageddon Album – 15.04.2024
",
            BioEn = @"DIARSIA is a hybrid deathcore band founded in late 2019 / early 2020. Their style is defined by dark tones and sudden breakdowns, with influences from metalcore, djent, thall, and slam. The “hybrid” identity also comes from combining raw heaviness with unique materials such as Vietnamese traditional instruments, EDM, phonk, and trap.

EX-MEMBERS:
• Ty Pham - bass (10/2020 - 06/2023)
• Nguyen Quoc Viet - guitar (02/2020 - 09/2020)
• Nguyen Shien - guitar (02/2020 - 05/2020)
• Nguyen Thao - bass (01/2020 - 09/2020)
• Tu Ly - drums (02/2020 - 04/2023)
• Ling Nguyen - guitar (01/2021 - 01/2025)

CURRENT MEMBERS:
• Khang Nguyen - vocals, frontman (01/2020 - present)
• Tuan Kiet - drums (09/2023 - present)
• Xuan Khoi - guitar (06/2020 - present)
• Dang Khoa - bass (01/2025 - present)

RELEASED WORKS:
1. Salvation - 10.12.2020
2. Purge (ft. Huy Gái from I’m Not Sure) - 25.07.2021
3. The Plague - 30.09.2022
4. Bloodmoon - 15.12.2022
5. Berzerker (ft. Ryu Miura from DIVINITIST JP) - 21.07.2023
6. Self Destruction (ft. Huỳnh Huy from District105) - 23.11.2023
7. Armageddon Album - 15.04.2024",
            HeroUrl = "/assets/bands/diarsia/hero.jpg",
            LogoUrl = "/assets/bands/diarsia/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 19
        },
        #endregion
        #region Band 20 — I'm Not Sure
        new Band
        {
            Id = 20,
            Name = "I'm Not Sure",
            Bio = "",
            HeroUrl = "/assets/bands/imnotsure/hero.JPG",
            LogoUrl = null,
            IsSecret = true,
            IsFeaturedOnHome = true,
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 20
        },
        #endregion
        #region Band 21 — Infernal Chaos
        new Band
        {
            Id = 21,
            Name = "Infernal Chaos",
            Bio = @"Infernal Chaos (IC) được thành lập năm 2004 tại Đài Bắc, Đài Loan và là một tên tuổi nổi bật trong làn sóng metal châu Á. Ban đầu được khởi xướng bởi hai guitarist Jesse “Black” Liu (cũng được biết đến qua ban nhạc huyền thoại Chthonic閃靈) và Mason, ban nhạc đã tạo nên bản sắc riêng bằng cách kết hợp metalcore, melodic death metal và groove metal. Âm nhạc của họ khai thác các chủ đề đen tối và mang tính xã hội như giết chóc, phản bội và sự vỡ mộng trước xã hội.

Đội hình hiện tại gồm:

• Vocalist: Kenney (tham gia từ 2016)
• Guitarist: Jesse “Black” Liu (Founder)
• Guitarist: Yu Jie
• Bassist: Mason Mick
• Drummer: Yu

IC nổi tiếng với các sản phẩm mạnh mẽ, được đầu tư chỉn chu như ""The Mask on Your Face"" và ""Society Psychopath"", thể hiện năng lượng thô ráp cùng khả năng kết hợp riff nặng với giai điệu bắt tai.

Ban nhạc từng tạm ngưng giai đoạn 2007-2013 nhưng đã trở lại mạnh mẽ hơn, tiếp tục phát triển và phát hành các sản phẩm vượt giới hạn. Các buổi diễn live của họ luôn thu hút chú ý, đặc biệt trong cộng đồng metal Đài Loan, đồng thời họ cũng đã biểu diễn cùng nhiều nghệ sĩ quốc tế, góp phần thúc đẩy phong trào metal tại châu Á.

Với phong cách đa dạng và cam kết về chất lượng sản xuất cao, Infernal Chaos vẫn là một trong những ban metal hàng đầu Đài Loan, thu hút người hâm mộ từ nhiều nơi trên thế giới.
",
            BioEn = @"Founded in 2004 in Taipei, Taiwan, Infernal Chaos (IC) is a powerhouse in the Asian metal scene. Initially formed by guitarists Jesse “Black” Liu, who is also known for his work with one of the most iconic metal acts in Taiwan, Chthonic 閃靈, and Mason, the band carved out a distinctive niche by blending metalcore, melodic death metal, and groove metal. Their music explores dark, socially conscious themes such as murder, betrayal, and disillusionment.

Current lineup:
• Vocalist: Kenney (joined in 2016)
• Guitarist: Jesse “Black” Liu (founder)
• Guitarist: Yu Jie
• Bassist: Mason Mick
• Drummer: Yu

IC is known for intense, high-quality releases like ""The Mask on Your Face"" and ""Society Psychopath.""

After a hiatus from 2007 to 2013, the band returned stronger, continued evolving, and has shared stages with international acts, contributing to Asia’s growing metal movement.",
            HeroUrl = "/assets/bands/infernalchaos/hero.jpg",
            LogoUrl = "/assets/bands/infernalchaos/logo.png",
            IsSecret = true,
            IsFeaturedOnHome = true,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 21
        },
        #endregion
        #region Band 22 — Longbez
        new Band
        {
            Id = 22,
            Name = "Longbez",
            Bio = "",
            HeroUrl = "/assets/bands/longbez/hero.jpeg",
            LogoUrl = "/assets/bands/longbez/logo.JPG",
            IsSecret = true,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 22
        },
        #endregion
        #region Band 23 — HappyCo
        new Band
        {
            Id = 23,
            Name = "Happyco.",
            Bio = @"happyco. (viết tắt của happy company) aka công ti vui vẻ là một ban nhạc Hardcore Punk, Post-hardcore và một chút Alternative - 4 thành viên đến từ Sài Gòn, Việt Nam. Được thành lập vào ngày 04.11.2018 bởi Thịnh (vocal/rhythm guitarist), Tè (bassist), Thầy (lead guitarist) và chính thức đi vào hoạt động năm 2022 sau khi tìm được mảnh ghép cuối cùng là Trúc (drummer). Định hướng cũng như mong muốn của happyco. là kết nối thêm nhiều người nghe từ nhiều dòng nhạc, sub-genre khác nhau. Từ đó có thể vượt qua khỏi rào cản cũng như định kiến từ ""genre"".

2. EP đầu tay ‘Displacement over Time (d/t)’

‘Displacement over Time (d/t)’ là EP đầu tay của happyco. (aka Công ti vui vẻ), gồm 7 bài hát thuộc các thể loại Hardcore Punk, Post-hardcore và Alternative. EP đã chính thức ra mắt trên mọi nền tảng vào ngày 22.08.2025 với track V/S/ON được release theo dạng single trước đó vào tháng 06.2024. Lyrics Video cho các ca khúc trong EP đã được đăng tải trên kênh YouTube của happyco. vào ngày 08.09.2025.

Được ấp ủ từ năm 2020, bắt đầu từ những bản demo tự sáng tác đầu tiên, nhưng mãi đến năm 2022 khi Trúc gia nhập nhóm với vai trò drummer, các sáng tác mới dần được hoàn thiện. Từ đó, Công ti đã bắt tay vào thu âm vào cuối năm 2024 cùng đội ngũ 1010 Network. Công đoạn mix, master và phát hành được hoàn thiện trong năm 2025. 7 bài trong EP được chia thành:

- A Side:
+ ‘V/S/ON’
+ ‘Ouroboros’
+ ‘Birthdays’
+ ‘Bajiquan Stance’
- B Side:
+ ‘Howl’
+ ‘Grapefruit Flower’
+ ‘1 in the Morning’

Nội dung của ‘Displacement over Time (d/t)’  là hành trình đấu tranh và quyết định phá vỡ vòng lặp tự hủy (break the cycle) - một quá trình giằng xé với chính bản thân để vượt qua sự thất bại, cô đơn, trống rỗng, căm ghét và nghi ngờ bản thân. Nhân vật chính trong EP đã chọn cách nhìn lại quá khứ (B Side), đối diện, và cuối cùng phá vỡ vòng lặp đó để trở thành một phiên bản tốt hơn của chính mình (‘Birthdays’, ‘Bajiquan Stance’), cũng giống như hình ảnh bẻ đôi con rắn cạp đang đuôi (‘Ouroboros’) - biểu tượng của vòng lặp và tự hủy .

Phong cách mà happyco. muốn hướng đến trong EP là sự tự do sáng tạo, không bị bó buộc trong bất kỳ genre cụ thể nào, nhưng vẫn giữ được nguồn năng lượng đặc trưng của hardcore. Công ti mong muốn EP ‘Displacement over Time (d/t)’ không chỉ là sản phẩm âm nhạc mà còn là một làn gió mới cho scene Hardcore Việt Nam, mang đến một nguồn năng lượng tích cực, mạnh mẽ mà không nhất thiết phải bạo lực hay cực đoan. Thông qua đó, Công ti mong muốn kết nối nhiều người đến với  Hardcore hơn, phá bỏ những định kiến tiêu cực về scene và mang nguồn năng lượng tươi mới đến với người nghe.",
            BioEn = @"happyco. (short for happy company), also known as Công ti vui vẻ, is a hardcore punk, post-hardcore, and slightly alternative band — four members from Saigon, Vietnam. Formed on 4 November 2018 by Thinh (vocals / rhythm guitarist), Te (bassist), and Thay (lead guitarist), the group went fully active in 2022 when Truc joined as drummer. happyco.’s aim is to connect more listeners from different scenes and sub-genres, so they can move past the barriers and stereotypes around the idea of ""genre.""

2. Debut EP ‘Displacement over Time (d/t)’

‘Displacement over Time (d/t)’ is happyco.’s debut EP: seven songs in hardcore punk, post-hardcore, and alternative. It was released on all platforms on 22 August 2025, after V/S/ON came out as a single in June 2024. Lyric videos for the EP tracks were published on happyco.’s YouTube channel on 8 September 2025.

The EP was nurtured from 2020, starting with the first self-written demos, but it was only in 2022, when Truc joined on drums, that new material steadily came together. From there the band recorded in late 2024 with the 1010 Network team; mixing, mastering, and release were completed in 2025. The seven tracks are grouped as:

• A side: V/S/ON, Ouroboros, Birthdays, Bajiquan Stance
• B side: Howl, Grapefruit Flower, 1 in the Morning

‘Displacement over Time (d/t)’ is about fighting to break a self-destructive cycle (break the cycle) — a struggle with oneself to move past failure, loneliness, emptiness, resentment, and self-doubt. The main character looks back at the past (B side), faces it, and finally breaks that loop to become a better version of themselves (‘Birthdays’, ‘Bajiquan Stance’), much like the image of snapping the ouroboros — the serpent biting its tail (‘Ouroboros’) — a symbol of the cycle and self-destruction.

happyco. want this EP to feel creatively free: not boxed into any single genre, but still carrying hardcore’s defining energy. They hope ‘Displacement over Time (d/t)’ is not only a musical release but a new wind for Vietnam’s hardcore scene — positive, forceful energy that does not have to mean violence or extremes. Through it they hope to connect more people with hardcore, push back on negative scene stereotypes, and bring a fresher energy to listeners.",
            HeroUrl = "/assets/bands/happyco/hero.jpg",
            LogoUrl = "/assets/bands/happyco/logo.png",
            IsSecret = false,
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 23
        },
        #endregion
        #region Band 24 — Volcate
        new Band
        {
            Id = 24,
            Name = "Volcate",
            Bio = "",
            HeroUrl = "/assets/bands/volcate/hero.png",
            LogoUrl = "/assets/bands/volcate/logo.png",
            IsSecret = true,
            IsFeaturedOnHome = true,
            LineupDay = LineupDay.HoDau,
            LineupPosition = 24
        },
        #endregion
        #region Band 25 — CLB Guitar Gỗ
        new Band
        {
            Id = 25,
            Name = "CLB Guitar Gỗ",
            Bio = @"CLB Guitar Gỗ Đại học Kiến trúc TP.HCM 

 CLB Guitar Gỗ là tập hợp những con người mê tiếng đàn mộc, thích ca hát và chẳng ngại quẩy hết mình. Tụi mình gặp nhau không chỉ để chơi nhạc, hát hò, mà còn để tán gẫu, làm trò và cùng tạo nên những khoảnh khắc vui vẻ.

 Ngoài những buổi sinh hoạt quen thuộc, tụi mình còn có hai sự kiện đặc biệt quan trọng là Gala Tháng và Gala Năm - những sân khấu siêu đặc biệt với chủ đề thay đổi liên tục, tha hồ cho mọi người bung năng lượng, khoe tài năng và cháy hết mình.

 Nếu bạn thích âm nhạc, muốn tìm một ngôi nhà vui vẻ, ấm áp và nhiều kỷ niệm đẹp thì “nhà Gỗ” chính là nơi dành cho bạn đó! Theo dõi các hoạt động sắp tới của CLB Guitar Gỗ tại: https://www.facebook.com/arcguitar
",
            BioEn = @"CLB Guitar Gỗ of Ho Chi Minh City University of Architecture is a student guitar community built around acoustic music, singing, and shared creative experiences.

Beyond regular club sessions, they host two major signature events each year: Gala Tháng and Gala Năm, with changing themes that encourage members to perform, express themselves, and fully enjoy the stage.

If you love music and want a warm, joyful creative home, “”nhà Gỗ”” is the place to be.",
            HeroUrl = "/assets/bands/clbguitargo/hero.jpg",
            LogoUrl = "/assets/bands/clbguitargo/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 25
        }
    #endregion
    }.AsReadOnly();
}

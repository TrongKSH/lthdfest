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
• Nguyễn Chánh Hiệp  - rhythm guitarist
• Lê Hoàng Minh Quân - lead guitarist
• Quan Vĩnh Kiện - lead vocalist
• Nguyễn Huy Khiêm -  bassist
• Trương Kim Trọng - drummer",
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
            Bio = @"BLACK INDUSTRY được thành lập vào cuối năm 2023, với chỉ 2 thành viên founder, Andie Christ và Jon Apollyon với âm thanh máy móc đanh đét và nặng nề của dòng nhạc Industrial Metal. 

Qua thời gian phát triển và ra mắt ca khúc single đầu tay trong EP 'Industry Whore': 'Braindead' với sự ủng hộ nhiệt tình từ người nghe, 2 thành viên quyết định lập band nhạc chính thức tên là Black Industry. 

Họ đã chiêu mộ các vị trí còn thiếu trong band để mang lại các màn trình diễn live chất lượng nhất, từ đó kết nạp thêm: Bush (lead guitar), Cbosi (bassist), Richfield (Percussionist) và Brian (Drummer). 

Đầu năm 2026, BLACK INDUSTRY chia tay 2 thành viên Jon Apollyon (guitar chính của band) và Cbosi (bassist của band). Vì tính chất công việc và học tập nên 2 thành viên này không thể tiếp tục đồng hành cùng band.

Ở thời điểm hiện tại band đã có có 1 vài thành viên mới để thay thế cho 2 vị trí còn sót đó là Luna (hát chính) và Hoàng Phương (guitar session) và X (bassist).

Black Industry  phát album đầu tay có tên 'My Tears Run BLACK’ vào giữa năm 2025. Album là một bản tuyên ngôn đậm chất Industrial Metal, album kết hợp nhiều yếu tố thể loại như Electro, Synthcore, Techno, Nu Metal, Alternative Metal, Symphonic Metal và Metalcore - tạo nên một không gian âm nhạc mạnh mẽ và độc đáo. 'My Tears Run BLACK' là hành trình khai phá xung đột nội tâm, tình yêu, xã hội và những mối quan hệ. Hiện có thể nghe album trên các nền tảng trực tuyến.

THÀNH VIÊN HIỆN TẠI:
•   Trung Kiên (Andie Christ) - vocalist, bassist 
•   Thái Huân (Brian) - drummer 
•   Ngọc Ánh (Luna) - vocalist 
•   Tiến Phát (Richfield) - percussionist 
•   Duy Anh (Bush) - guitarist 
•   Hoàng Phương - guitarist session 

THỂ LOẠI: Alternative Metal, Industrial Metal, Nu Metal

NHỮNG SẢN PHẨM ĐÃ RA MẮT:
Album “My Tears Run Black”: https://youtu.be/No9uWqxz2D0?si=GfPbA4_LyJSo8EZv
The Letter MV:https://youtu.be/fZ_rHDKggv8?si=XKVbxOVI7-3nYiZi
Heartless MV:https://youtu.be/F9ld3NGyjXA?si=SHId5Lry786wmaR7
Braindead MV:https://youtu.be/P_SCJHSgaNw?si=9lmeSKof5bsswmVH",
            HeroUrl = null,
            LogoUrl = "/assets/bands/blackindustry/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 4
        },
        new Band
        {
            Id = 5,
            Name = "Cút Lộn",
            Bio = @"Cút Lộn is a Vietnamese hardcore band that was formed in 2018. It started as a crossover thrash band with their debut album Xào Ke in early 2019, which was followed by 3 more singles during 2020-2023.

In early 2024 the band changed the vocalist and the music direction towards being heavier, darker and more chaotic. With this new lineup the band released their second studio album “Bắc Thảo” in August 2024 and then the third one called “Dzữa” in October 2025.

Cút Lộn is known for their high energy live performances and sarcastic, yet poetic lyrics written in Vietnamese. Almost every song from their crossover thrash era has a music video to it released on their YouTube channel and the band also released two music videos for the newer songs from Bắc Thảo.

The band played a lot of shows all around Vietnam and also in South Korea, Thailand, Malaysia, Philippines and Cambodia. Some of their notable performances include Rock Alarm Festival 2024 in Bangkok, Thailand and Baybeats Festival 2025 in Singapore.

Their upcoming tour in November and December 2025 also includes the dates in China, Malaysia and Philippines.

Lineup:
  Vui Qá: Vocal
  Quang Sọt: Guitar
  Nguyên Lê: Bass
  Sergey: Drums",
            HeroUrl = "/assets/bands/cutlon/hero.jpg",
            LogoUrl = "/assets/bands/cutlon/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 5
        },
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
            HeroUrl = "/assets/bands/diesofar/hero.JPG",
            LogoUrl = "/assets/bands/diesofar/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 6
        },
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
            HeroUrl = null,
            LogoUrl = "/assets/bands/hypertension/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 9
        },
        new Band
        {
            Id = 10,
            Name = "Kinh",
            Bio = @"KINH was formed in mid-2023 in Saigon, Vietnam, by Huy Goodman, Huyên Blaster, and Siêu Dê Cụ with the intention of creating and playing extreme music. We play death metal influenced by genres such as hardcore, grindcore, and even a touch of hip hop. We believe there are no limitations in music and let our imagination go beyond boundaries when creating heavy music.

Genre: Death Metal
Location: Saigon, Vietnam
Youtube: @Kinh_SaigonDeathMetal
Facebook: www.facebook.com/KinhSaigonDeathMetal/ 
Instagram: www.instagram.com/kinh_saigondeathmetal/
Email: Kinhextreme.official@gmail.com

Lineup:
• Siêu DÊ cụ - No Treble/Voice
• Huy Goodman - Guitar
• Huyên Blaster - Drum

NỘ - Single 2024
Releases: May 13th, 2024

VÔ - Single 2025
Releases: Aug 8th, 2025
",
            HeroUrl = "/assets/bands/kinh/hero.jpg",
            LogoUrl = "/assets/bands/kinh/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 10
        },
        new Band
        {
            Id = 11,
            Name = "Nhao",
            Bio = @"NHẠO là ban nhạc rock Việt Nam được thành lập vào ngày 20/09/2020 và hiện tại đang hoạt động ở Sài Gòn với 4 thành viên ban nhạc: 
• Trọng Nhân (Band Leader - Vocalist)
• Thanh Duy (Rhythm Guitarist)
• Quốc Bảo (Drummer)
• Bảo Long (Bassist) 

NHẠO trong Hán - Việt  樂 có nghĩa là “nhạc” và trong tiếng Nôm có nghĩa là “nhạo báng”, “chế nhạo”.  Âm nhạc của NHẠO mang sự ngông cuồng, kiêu ngạo của tuổi trẻ để nói lên góc nhìn của ban nhạc đối với những vấn đề khác nhau trong cuộc sống.

Luôn học hỏi và nâng cao bản thân từng ngày, NHẠO sẽ cố gắng phát triển để đạt được những ước mơ mà cả đám hướng tới. 
",
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
•   Nguyễn Thành Các - guitar rhythm 
•   Trần Hoàng Phúc - guitar lead 
•	Đinh Bảo Nam - lead vocalist
•	Trương Kim Trọng - bass, backing vocal
•	Võ Phúc Hoan - drummer",
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
            HeroUrl = "/assets/bands/thanhduc/hero.jfif",
            LogoUrl = "/assets/bands/thanhduc/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 14
        },
        new Band
        {
            Id = 15,
            Name = "Under Pressure",
            Bio = @"Được thành lập vào ngày 01.06.2024 tại Sài Gòn, Việt Nam, là một ban nhạc theo đuổi dòng nhạc Metallic Hardcore bao gồm những cá nhân trẻ tuổi, đầy nhiệt huyết. Hiện tại, đội hình gốc của ban nhạc bao gồm: Đạt lê (Drummer), Jackk Nguyễn (Lead Guitar) và Hiếu Minh (Bassist). Gần đây, ban nhạc đã bổ sung thêm vị trí Rhythm Guitar là Khoa (aka Bác Trịnh) và Vocalist là Ares

Vào ngày 31.12.2025 vừa rồi, band đã cho ra mắt EP “Omnipotent”. Qua EP lần này, Under Pressure chúng tôi muốn truyền tải những thông điệp, cảm xúc và tinh thần mà hiếm khi được nhắc đến trong những dòng nhạc đại chúng. Lấy cảm hứng từ các cuộc chiến tranh đã diễn ra xuyên suốt hằng thế kỷ, từng bài trong EP mang những góc nhìn của các bên trong thời chiến. Bài #3 - “Dethroned” là những lời kêu gọi, tinh thần của quân kháng chiến chống lại cường quyền tàn bạo. Bài #2 - “Seraph” mang góc nhìn của những người lính chiến đấu, sẵn sàng chết vì tổ quốc khi đất nước họ bị xâm lược bởi các thế lực thù địch.

Đúng với cái tên “Omnipotent”, ban nhạc muốn nhấn mạnh rằng đằng sau những âm thanh ồn ào và bạo lực ấy là lời nhắn nhủ về một phong cách sống độc lập, có chính kiến, dám đứng lên và hành động, thực hiện những gì bản thân mình tin tưởng.
",
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
            Name = "9xacly",
            Bio = "",
            HeroUrl = "/assets/bands/9xacly/hero.png",
            LogoUrl = "/assets/bands/9xacly/logo.png",
            LineupDay = LineupDay.HoDau,
            LineupPosition = 18
        },
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
            HeroUrl = "/assets/bands/diarsia/hero.jpg",
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
            Name = "CLB Guitar Gỗ",
            Bio = @"CLB Guitar Gỗ Đại học Kiến trúc TP.HCM 

 CLB Guitar Gỗ là tập hợp những con người mê tiếng đàn mộc, thích ca hát và chẳng ngại quẩy hết mình. Tụi mình gặp nhau không chỉ để chơi nhạc, hát hò, mà còn để tán gẫu, làm trò và cùng tạo nên những khoảnh khắc vui vẻ.

 Ngoài những buổi sinh hoạt quen thuộc, tụi mình còn có hai sự kiện đặc biệt quan trọng là Gala Tháng và Gala Năm - những sân khấu siêu đặc biệt với chủ đề thay đổi liên tục, tha hồ cho mọi người bung năng lượng, khoe tài năng và cháy hết mình.

 Nếu bạn thích âm nhạc, muốn tìm một ngôi nhà vui vẻ, ấm áp và nhiều kỷ niệm đẹp thì “nhà Gỗ” chính là nơi dành cho bạn đó! Theo dõi các hoạt động sắp tới của CLB Guitar Gỗ tại: https://www.facebook.com/arcguitar
",
            HeroUrl = "/assets/bands/clbguitargo/hero.jpg",
            LogoUrl = "/assets/bands/clbguitargo/logo.png",
            LineupDay = LineupDay.LongTranh,
            LineupPosition = 25
        }
    ];
}

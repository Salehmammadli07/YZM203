# Giriş

Bir programı anlamanın başlangıç noktası, ekranda görünen sonucu o sonuca ulaşan işlemlerle ilişkilendirmektir. Doğru sonuç hangi girdiler için geçerlidir? Değişkenler neyi temsil eder? İşlem sırası değişirse ne olur? Bu kitap, bu soruları C# ile çözülen 100 bağımsız problem üzerinden ele alır. Giriş bölümü, kullanılan yaklaşımın kavramsal çerçevesini kurar ve problemleri etkin biçimde çalışmak için bir yöntem önerir. Bütün teknikleri önceden bilmeniz beklenmez; yeni kavramları ilgili problemle birlikte öğrenip önceki kazanımlara bağlamanız beklenir.

## Prosedürel Programlama Nedir?

Prosedürel programlama, bir problemi belirli görevleri yerine getiren işlem birimlerine ayırarak çözme yaklaşımıdır. Bu birimler, gerektiğinde çağrılan prosedürler veya metotlar olarak düzenlenir. Örneğin bir ölçüm raporu hazırlamak; veriyi okuma, doğrulama, hesaplama ve sonucu sunma görevlerine ayrılabilir. Her görev açık bir giriş, işlem ve çıkış ilişkisine sahip olduğunda ana programın akışı daha kolay izlenir. Ayrıştırmanın ölçütü yalnız kod uzunluğu değildir; bir parçanın neyi yaptığı ve hangi kuralları koruduğu da belirleyicidir.

Prosedür sözcüğü genel olarak çağrılabilir bir işlem birimini anlatır. Bazı dil ve kaynaklarda sonuç döndüren birimler fonksiyon, sonuç döndürmeyenler prosedür diye ayrılır. C# bağlamında metot, bir tür içinde tanımlanan çağrılabilir kod birimidir. Kitapta üst düzey ifadelerle yazılmış konsol programları ve `static` yerel fonksiyonlar kullanılır; bu yerel fonksiyonlar da prosedürel ayrıştırma işlevi görür. Öğretim anlatımında bunların görevlerinden metot olarak söz edilir. Sınıf tasarlamadan alt görevler kurabilmek, C# dilinin nesneye yönelik olanaklarını reddetmek anlamına gelmez.

Değişken, belirli bir türdeki değeri adıyla kullanmamızı sağlar. Tür, hangi değerlerin ve işlemlerin anlamlı olduğunu sınırlar. Bir adet için tamsayı, bir karar için `bool`, bir metin için `string` seçilebilir. Değişken adını yalnız kısa olduğu için seçmek yerine anlamına göre seçin: `gecerliAdet`, saklanan veri sayısını kapasiteden ayırır. Programın belirli bir andaki ilgili değerleri birlikte onun durumunu oluşturur. Durumu okumak, yalnız sonuç değişkenine bakmak değildir; sayaçları, dizileri ve işlem konumunu da birlikte değerlendirmektir.

Sıralı yürütmede bir işlem tamamlanır, ardından sonraki işleme geçilir. Atama, eşitlik bildiren matematiksel bir önerme değil, bir değeri güncelleyen işlemdir. Aşağıdaki bağımsız örnekte ikinci atama, ilk atamanın değiştirdiği durumu kullanır:

```csharp
using System;

int kalan = 12;
int kullanilan = 5;
kalan = kalan - kullanilan;
kullanilan = kalan + 1;
Console.WriteLine($"Kalan: {kalan}, kullanılan: {kullanilan}");
```

Çıktı `Kalan: 7, kullanılan: 8` olur. Başlangıç değerleri ile son değerler farklıdır. Satırları yer değiştirip yeniden çalıştırdığınızda aynı sonucu bekleyemezsiniz. İzleme sırasında her atamadan sonraki değerleri ayrı satıra yazmak bu bağımlılığı görünür kılar. Büyük bir uygulamada da aynı düşünce geçerlidir: henüz doğrulanmamış veriyi sonuç üretiminde kullanmak, yanlış bir yürütme sırası oluşturabilir.

Karar yapısı, koşulun doğru veya yanlış olmasına göre yürütülecek işlemi seçer. `if`, alternatif yolları; `switch`, belirli seçenekleri düzenlemeye yardım eder. Yineleme ise bir işlem grubunun koşula veya veri kümesine bağlı olarak tekrar yürütülmesidir. `for`, `while` ve `do / while` farklı denetim düzenleri sunar. Döngünün başlangıcını, devam koşulunu ve ilerleme adımını birlikte düşünmek gerekir. Sıfır elemanlı veri, bir kez yürütme ve hiç yürütmeme arasındaki farkı ortaya çıkarır. Sonlanmanın gerekçesi, yalnız kodun bugün çalışmış olmasıyla açıklanamaz.

Parametre, işlem biriminin dışarıdan beklediği bilgiyi tanımlar; çağrıda verilen somut değer argümandır. Dönüş değeri, hesaplanan sonucu çağırana taşır. `return` çağrıyı bitirip denetimi çağırana verir; `void` normal bir sonuç değeri döndürülmediğini belirtir. Ekrana yazdırma ile değer döndürme aynı işlem değildir. Hesabı konsoldan ayırmak, aynı işlemi başka bir programda kullanmayı ve doğrudan sınamayı kolaylaştırır. Örnekte yalnız miktar ve eşik arasındaki ilişki hesaplanır:

```csharp
using System;

Console.WriteLine(EsigiGectiMi(18, 15));
Console.WriteLine(EsigiGectiMi(15, 15));

static bool EsigiGectiMi(int miktar, int esik)
{
    return miktar > esik;
}
```

Çıktılar sırayla `True` ve `False` olur; eşitlik, kesin aşma değildir. Bu işlem dış değişken kullanmaz ve veriyi değiştirmez. Her metot için böyle bir sözleşme düşünün: parametrelerin anlamı, kabul edilen değerler, sonuç ve varsa yan etkiler. Bir dizi parametresinde hücre değişikliği çağıranın verisini etkileyebilir; parametrenin değer olarak aktarılması, dizinin kopyalandığı anlamına gelmez. Kitabın metot bölümünde bu ayrım, `ref` ve `out` kullanımıyla birlikte ayrıntılandırılır.

## Neden Öğrenilmelidir?

Prosedürel yaklaşım, problem ayrıştırmayı gözlemlenebilir adımlarla öğretir. Bir rapor isteğinde önce geçerli kaydın ne olduğunu tanımlamak, sonra hesaplamak, en son yazmak gerekir. Bu görevleri ayırdığınızda bir dosya biçimi değişikliği ile bir hesaplama değişikliğinin farklı sorumluluklar olduğunu görebilirsiniz. Her parçanın bağımsız açıklanabilmesi, parçaların doğru bağlandığını kendiliğinden garanti etmez; bu yüzden ana akışın sırası ve veri aktarımı da incelenmelidir. Ayrıştırma, bütün çözümün ilişkilerini görünür tutmalıdır.

Algoritmik düşünme, belirsiz bir amacı uygulanabilir ve sonlanan adımlara dönüştürmektir. “En uygun seçimi bul” ifadesinde uygunluk ve iyilik ölçüsü tanımlanmadan algoritma kurulamaz. Son gruptaki üretim planı belirli bir öncelik politikasını uygular; görev ataması ise tanımlanmış kısıtlar altında toplam maliyeti en aza indirir. Bu iki hedefi ayırabilmek, kullanılan döngüden daha temel bir beceridir. Yerel olarak ucuz görünen bir seçim, diğer seçenekleri kapatarak bütün çözümü pahalı hale getirebilir.

Doğrulama, programın kabul ettiği girdilerin sözleşmeye uyup uymadığını denetlemektir. Bir sayının ayrıştırılabilmesi onun problem için geçerli olduğunu göstermez; eksi bir adet, sayısal açıdan okunabilirken iş kuralına aykırı olabilir. Benzer biçimde bir ürün kodu biçimce geçerli, fakat katalogda bulunmuyor olabilir. Kitabın dosya problemleri biçim, aralık, benzersizlik ve ilişki denetimlerini ayırır. Sonuçların doğrulama tamamlanınca üretilmesi, eksik veriyle doğruymuş gibi görünen rapor yayımlanmasını önlemeye yardımcı olur.

Hata ayıklama, gözlenen davranış ile beklenen davranış arasındaki ilk ayrılığı bulmayı gerektirir. Sonucun yanlış olduğunu bilmek başlangıçtır; hangi adımda hangi varsayımın bozulduğunu göstermek çözümün yolunu açar. Bir toplamın hatalı çıkması, toplama işleminden önce yanlış hücrenin okunmasından kaynaklanabilir. Küçük girdiler, ara değer tabloları ve hata konumları bu nedenle değerlidir. Rastgele satır değiştirmek yerine hatayı tekrarlayan girdiyi saklayın, beklenen sonucu bağımsız hesaplayın ve düzeltmeden sonra aynı girdiyi yeniden deneyin.

Maliyet analizi, veri büyüdüğünde gereken işlemlerin ve belleğin nasıl değiştiğini inceler. n elemanı bir kez taramak ile her elemanı diğer bütün elemanlarla karşılaştırmak aynı büyüme davranışını göstermez. Sonuçları karşılaştırırken hangi işlemin sayıldığını belirtin; değer karşılaştırması, veri hareketi ve süre farklı ölçülerdir. Sınır analizi ayrıca tür kapasitesini, dizi boyutunu, boş veriyi ve dosya büyüklüğünü kapsar. Sayısal taşma, doğru bir matematiksel formülün bilgisayardaki sınırlı türle yanlış uygulanmasına yol açabilir.

Bu temel, nesneye yönelik programlamaya geçişte de kullanılır. Bir rezervasyonun başlangıcı, bitişi ve çakışma denetimi sürekli birlikte kullanılıyorsa veri ile davranışı aynı sorumlulukta toplama gereksinimi doğar. Sınıf tasarımı bu ilişkinin korunmasına yardımcı olabilir; ancak metotların sözleşmesi ve algoritmanın doğruluğu yine gereklidir. Prosedürel yaklaşım küçük hesaplar ve belirgin işlem hatları için elverişlidir. Uzun ömürlü durum ve çok sayıda ilişki başka düzenlemeler gerektirebilir. Hiçbir paradigmanın her problemde üstün olduğu varsayılmamalıdır.

## Farklı Programlama Paradigmaları Nelerdir?

Programlama paradigması, çözümü hangi kavramlarla kurduğumuzu ve kodu nasıl düzenlediğimizi anlatan yaklaşım ailesidir. Emirsel programlama, durumu değiştiren komutlarla işlemlerin nasıl yapılacağını belirtir. Prosedürel programlama, bu emirsel işlemleri çağrılabilir görevler etrafında düzenler. Dolayısıyla bu iki kavram eş düzeyde ve birbirinden kopuk seçenekler değildir. Bir prosedürel program emirsel özellikler taşırken, bir nesnenin metodunun içi de sıralı atamalar ve döngülerle yazılabilir.

Nesneye yönelik yaklaşım, veriyi ve o veri üzerindeki davranışları nesneler çevresinde düzenler. Sorumluluk, kapsülleme ve nesneler arasındaki ilişkiler tasarımın merkezindedir. Öğrenci numarası, notlar ve başarı hesabı farklı dizilerde tutulduğunda aynı dizinin doğru hücrelerini eşlemek gerekir. Bu verileri uygun bir öğrenci modeli içinde bir araya getirmek tutarlılığın korunmasını kolaylaştırabilir. Bunun bedeli yeni türler ve ilişkiler tasarlamaktır; yalnız her fonksiyonu bir sınıfa taşımak iyi bir model oluşturmaz.

İşlevsel yaklaşım, hesaplamayı işlevlerin uygulanması ve birleştirilmesi üzerinden düşünür; değişmeyen veriyi ve yan etkilerin sınırlandırılmasını öne çıkarır. Saf işlev, aynı girdiler için aynı sonucu verir ve gözlenebilir yan etki oluşturmaz. Önceki eşik örneği bu düşünceye yakındır; dosyaya yazan bir işlem ise yan etkilidir. C# içinde işlev değerleri ve lambda ifadeleri kullanılabilir; F# bu yaklaşımı daha belirgin biçimde destekler. Her lambda ifadesi saf değildir: dışarıdaki değişkeni artıran bir lambda yine durum değiştirir.

Bildirimsel yaklaşım, istenen sonucu veya ilişkiyi ifade etmeye ağırlık verir; bütün yürütme adımlarını tek tek kullanıcı yazmaz. SQL sorgusu, hangi kayıtların seçileceğini belirtir; veritabanı motoru yürütme planını belirler. C# içindeki LINQ, veriler üzerinde sorgu ve dönüşüm ifadeleri sağlar. “Pozitif değerleri say” isteği bir sorguyla ifade edilebilir. Bu açıklık, yürütme maliyetini düşünme gereğini ortadan kaldırmaz; kaynağın türü ve sorgunun ne zaman yürütüldüğü davranışı etkileyebilir.

Mantıksal yaklaşımda olgular ve kurallar tanımlanır, bir sorgunun bu ilişkilerden çıkarılıp çıkarılamadığı araştırılır. Prolog, bu yaklaşımın bilinen örneğidir. Örneğin üst iş ilişkileri ve bir atalık kuralı tanımlanarak iki iş arasında dolaylı bağ olup olmadığı sorulabilir. C# bu çıkarım düzenini yerleşik Prolog sözdizimiyle sunmaz; benzer gereksinim bir kural motoruyla veya açık algoritmayla karşılanabilir. Mantıksal yaklaşım bildirimsel düşünceyle ilişkilidir, fakat onu genel sorgu ifadeleriyle aynı mekanizma olarak değerlendirmemek gerekir.

Olay güdümlü yaklaşım, çalışmanın ne zaman başlatıldığını ve etkileşimlerin nasıl düzenlendiğini ele alır. Düğmeye basılması, zamanlayıcının tetiklenmesi veya bir mesajın gelmesi ilgili işleyiciyi çalıştırabilir. C# arayüzlerinde olaylar ve temsilciler bu düzeni destekler. İşleyicinin içi prosedürel adımlardan oluşabilir; olay kaynağı ise bir nesne olabilir. Olay güdümlü düzen, hesaplama yönteminden farklı bir boyut taşır. İşlem sırası ve paylaşılan durumun yönetimi burada da önemlidir.

| Yaklaşım | Temel odak | Örnek dil veya araç | C# ile ilişki |
| --- | --- | --- | --- |
| Emirsel | Durum değiştiren adımlar | C, C# | Atama, koşul ve döngü |
| Prosedürel | Görevlere ayrılmış işlemler | C, Pascal | Metot ve yerel fonksiyon |
| Nesneye yönelik | Veri ve davranışın sorumluluğu | C#, Java | Sınıflar ve nesneler |
| İşlevsel | İşlevler ve veri dönüşümü | F#, Haskell | Saf hesaplar, lambda |
| Bildirimsel | İstenen sonuç ve ilişkiler | SQL | LINQ sorguları |
| Mantıksal | Olgulardan ve kurallardan çıkarım | Prolog | Kural motoru ile uyarlama |
| Olay güdümlü | Olaylara verilen yanıt | C# arayüzleri | Olaylar ve işleyiciler |

Tablo bir dil sınıflandırma sınavı değildir; odakları karşılaştırmak içindir. Kategorilerin tümü birbirini dışlamaz ve birçok dil birden fazla yaklaşımı destekler. C# ile nesneye yönelik bir uygulamada prosedürel bir hesaplama, işlevsel bir dönüşüm ve olay işleyicisi birlikte bulunabilir. Bir programın paradigmasını yalnız kullandığı dilin adına veya tek bir sözdizimi öğesine bakarak belirlemek, tasarımın önemli özelliklerini gözden kaçırır.

## Paradigmalara Hakim Olmak Neden Önemlidir?

Farklı paradigmaları tanımak, problemi uygun sorularla incelemeyi sağlar. İşlemler belirgin bir sırada mı ilerliyor? Verinin geçerliliği bir nesnenin yaşamı boyunca mı korunmalı? Amaç bir koleksiyondan sonuç seçmek mi, ilişkilerden çıkarım yapmak mı? Bu sorular, yaklaşım seçimini alışkanlıktan tasarım kararına dönüştürür. Küçük bir konsol hesabını büyük bir nesne hiyerarşisine dönüştürmek gereksiz olabilir; çok sayıda ilişkiyi tek uzun prosedürde yönetmek de bakım yükünü artırabilir.

Farklı kodları okuyabilmek, aynı isteğin başka biçimlerde ifade edildiğini görmeyi gerektirir. Aşağıdaki bağımsız örnek, pozitif eleman sayısını hem açık döngüyle hem LINQ ile hesaplar:

```csharp
using System;
using System.Linq;

int[] degerler = { -2, 0, 4, 7 };
int adet = 0;
foreach (int deger in degerler)
{
    if (deger > 0) adet++;
}
int sorguAdedi = degerler.Count(deger => deger > 0);
Console.WriteLine($"Döngü: {adet}, sorgu: {sorguAdedi}");
```

İki sonuç da 2'dir. İlk biçim sayaç güncellemesini gösterir; ikinci biçim seçme koşulunu ve sayma isteğini öne çıkarır. `Count` bir yöntem çağrısıdır ve bu dizide koşulu değerlendirerek sonuç üretir. Kısa yazılması, işlem yapılmadığı veya her kaynakta daha hızlı olduğu anlamına gelmez. Kitapta açık döngüler kullanılması, algoritmanın adımlarını öğrenme amacına hizmet eder. Hazır araçlarla çalışırken de sonuç, yan etki ve maliyet soruları sorulmalıdır.

Bakım ve test açısından amaç, değişikliğin etkisini sınırlayabilmektir. Saf bir hesaplama kolayca farklı girdilerle çağrılabilir; durum taşıyan bir nesne başlangıç ve geçiş kurallarıyla sınanır. Olay işleyicisini veri okuma ve hesaplamadan ayırmak, arayüz olmadan test yapmayı kolaylaştırır. Ekipte “önkoşul”, “yan etki”, “sorumluluk” ve “durum geçişi” sözcüklerinin ortak anlamda kullanılması anlaşmazlıkları azaltır. Ortak dil, herkesin aynı paradigmayı seçmesini değil, kararının nedenini açıklayabilmesini gerektirir.

Veri ile davranışın ilişkisini kavramak, sonraki öğrenmenin merkezindedir. Paralel dizileri birlikte sıralarken aynı kayda ait hücrelerin ayrılmaması gerekir. Nesneye yönelik model bu bütünlüğü bir türün sorumluluğu olarak ele alabilir; işlevsel dönüşüm özgün veriyi değiştirmeden yeni sonuç oluşturabilir. Her düzenin sağladığı güvenceler ve taşıdığı maliyetler vardır. Hakimiyet, bütün yaklaşımları aynı anda kullanmak değil, sorunu tanıyıp seçilen yaklaşımın sınırlarını açıklamaktır.

## Bu Kitap Nasıl Kullanılmalıdır?

Kitap, onar soruluk on bölümde kolaydan zora ilerler. İlk bölüm işlem sırası, değişken, temel koşul, döngü ve diziyle başlar. İkinci bölüm birleşik karar kurallarını, üçüncü bölüm döngüleri ve sayı algoritmalarını, dördüncü bölüm iç içe döngülerle kısıtlı aramayı geliştirir. Beşinci bölüm dizilerde veri işleme ve aramaya, altıncı bölüm sıralama ve algoritma karşılaştırmalarına ayrılır. Yedinci bölüm metin işleme ve ayrıştırma, sekizinci bölüm metotlar ve özyineleme, dokuzuncu bölüm dosyalar ve ilişkili veri kümeleri üzerinde çalışır. Onuncu bölüm önceki becerileri birleşik uygulamalar ve optimizasyonla tamamlar.

Her problem kendi sözleşmesi ve tam çözümüyle bağımsızdır; önceki problemin programını çalıştırmayı gerektirmez. Bununla birlikte öğrenme sırası ve önkoşullar vardır. Dosyanın başındaki önkoşul kimlikleri, kullanılan kazanımlara geri dönmeniz için yol gösterir. Bir örnekte özyineleme veya dizi sınırı anlaşılmıyorsa ilgili önceki kazanımı yeniden çalışın. Son gruptaki dinamik programlama, çizge ve geri izleme kavramları problem içinde açıklanır; kitabın başında bunların tümüne hakim olmanız beklenmez. Zorluk yalnız satır sayısıyla ölçülmez.

Çözümü açmadan önce problem tanımını okuyun, girdi aralıklarını ve beklenen çıktı biçimini yazın. Küçük bir örneği elle çözün; hangi ara bilgilerin gerektiğini belirleyin. Ardından algoritmayı kendi cümlelerinizle kurun. Kendi çözümünüz kitapla farklı olabilir; karşılaştırmayı metnin aynı olması üzerinden değil, sözleşmeyi karşılaması üzerinden yapın. Örneğin eşit değerlerde ilk konumu koruma şartı varsa herhangi bir doğru değeri bulmak yeterli değildir. Hata davranışı da çözümün parçasıdır.

Adım izleme için girdiyi küçük tutun. Bir tabloda yürütülen satırı, koşul sonucunu, sayaçları ve değişen hücreleri kaydedin. Döngüye girmeden önceki durum ile ilk turun sonunu ayrı gösterin. Özyinelemede çağrı parametrelerini ve dönüş sonuçlarını izleyin. Bir adımın neden doğru olduğunu açıklayamıyorsanız yalnız son çıktıyı ezberlemeyin. Geçerli veri sayısı ile dizi kapasitesi, kimlik ile dizin, boş kayıt ile dosya sonu gibi ayrımları özellikle işaretleyin; son problemlerde bu ayrımlar birlikte kullanılır.

C# kodunu bağımsız bir .NET 10 konsol projesinin `Program.cs` dosyasında çalıştırın. Her problem için tek çözümü kopyalayın; farklı programların üst düzey ifadelerini aynı dosyada birleştirmeyin. Konsol girdilerini örnekteki sırayla verin. Dosyalı sorularda verilen içerikleri kendi örnek klasörünüzde oluşturun, yolları ve argüman sırasını kontrol edin. `dotnet run --` sonrasındaki argümanlar programa aktarılır. Ondalık nokta, tarih biçimi ve çıktı satırları için ilgili sözleşmeyi izleyin; görünen küçük biçim farkları veri yorumunu değiştirebilir.

Normal örneklerden sonra sınır durumlarını deneyin: boş veri, tek eleman, eşit değerler, alt ve üst sınırlar, geçersiz son kayıt ve bulunamayan ilişki. Beklenen sonucu çalıştırmadan önce belirleyin. Rapor yazan örneklerde girişlerin korunmasını ve mevcut raporun ezilmemesini inceleyin; yeni dosya adları kullanın. Derlenen kodun çalışması, bütün girdiler için doğruluk kanıtı değildir. Testler belirli hataları ortaya çıkarır; algoritma açıklaması, korunan kurallar ve sonlanma gerekçesi daha genel güven oluşturur.

Son olarak kazanımları kendi cümlelerinizle açıklayın ve alıştırmaları çözün. Bir alıştırma yalnız yeni çıktı istemez; değişikliğin veri düzeni, sözleşme ve maliyet üzerindeki etkisini de düşündürür. Son grupta tekrar birlikte kullanılan veri ve davranışları listeleyin: rezervasyonun aralığı, stokun ilişkileri veya atamanın uygunluk kuralı gibi. Bunları hangi sorumlulukların koruyabileceğini tartışmak nesneye yönelik programlamaya hazırlıktır. Bu kitabın başarısı, 100 çözümü kopyalamakla değil, yeni bir problemi açık adımlara ayırıp çözümünüzü gerekçelendirebilmekle ölçülür.

## Kaynaklar

Kavramları derinleştirmek ve dil ayrıntılarını incelemek için aşağıdaki birincil öğretim kaynaklarına başvurabilirsiniz:

- [Microsoft Learn: C# metotları](https://learn.microsoft.com/en-us/dotnet/csharp/methods) - parametreler, argümanlar, dönüş ve değer/referans aktarımı.
- [Microsoft Learn: C# ile LINQ](https://learn.microsoft.com/en-us/dotnet/csharp/linq/) - sorgu ifadeleri ve koleksiyon işlemleri.
- [Microsoft Learn: F# nedir?](https://learn.microsoft.com/en-us/dotnet/fsharp/what-is-fsharp) - işlevsel yaklaşım ve diğer yaklaşımlarla birliktelik.
- [Learn Prolog Now!: Olgular, kurallar ve sorgular](https://lpn.swi-prolog.org/lpnpage.php?pageid=lpn-htmlse1&pagetype=html) - mantıksal yaklaşımın temel yapıları.

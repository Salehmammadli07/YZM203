# Prosedürel programlama

Bu çalışma, MYAZ205 kapsamında nesneye yönelik programlamaya geçmeden önce değişken, işlem sırası, koşul, döngü, dizi, metin işleme, metotlar ve dosyalarla problem ayrıştırma becerilerini geliştirir. Kolaydan zora ilerleyen 10 bölümde toplam 100 bağımsız, C# ile çözümlü problem hazırlanmıştır.

Markdown dosyaları ana kaynaktır. Word belgesinde kalıcı içerik düzenlemesi yapmak yerine ilgili Markdown dosyasını değiştirip yeniden derleyin. Derleme bütün problemleri yeniden okur; başlıkları, C# kodunu, tabloları, kazanımları ve şekilleri günceller.

[assets/cover.png](assets/cover.png) kitabın ilk sayfasına tam sayfa kapak olarak yerleştirilir. Kapak A4 boyutunda ayrı bir Word kesitidir; tüm kenar boşlukları sıfırdır, görsel sayfanın `(0, 0)` noktasından başlar ve kapakta sayfa numarası gösterilmez. İç sayfaların mevcut boyutu ve kenar boşlukları korunur. Görsel değiştirildiğinde aynı derleme komutu yeni kapağı yükler. Kapak dosyası bulunmayan örnek kitaplarda metin kapağı kullanılır.

Word belgesindeki **Normal** stil iki yana yaslıdır. Başlıklar, C# kodları, tablo metinleri (`TableText`) ve dizin stilleri kendi sola hizalı düzenini korur; yeniden derlemeler bu stil ayarlarını uygular. Bölüm sonları ayrı boş paragraflarda tutulur; böylece önceki bölümün son metin satırında gereksiz boşluk genişlemesi oluşmaz.

Her onluk grup bir bölümdür; toplam 10 bölüm vardır. Bölüm adları [chapters.md](chapters.md) dosyasından yüklenir. İçindekilerde bölüm başlıkları birinci, problem başlıkları ikinci düzeyde yer alır. Her bölüm yeni **çift numaralı sayfadan** başlar. Bunun için C# derleyici Word'ün `evenPage` bölüm sonlarını üretir; içerik uzadığında gerekirse araya boş sayfa eklenir. Sayfa numaralandırması kitap boyunca kesintisizdir. Bölümün ilk problemi bölüm başlığıyla aynı sayfada, sonraki problemler yeni sayfada başlar. Başlık veya problem değişikliğinden sonra normal derleme komutunu çalıştırmak bu düzeni yeniden kurar.

Prof. Dr. Zafer CÖMERT adına hazırlanan [Önsöz](front-matter/01-onsoz.md) ve öğretici [Giriş](front-matter/02-giris.md), problem koleksiyonundan ayrı Markdown kaynaklarıdır. Kitap sırası **kapak, Önsöz, İçindekiler, Şekil dizini, Giriş, on bölümde 001–100 problemleri, arka kapak** biçimindedir. Giriş; prosedürel yaklaşımı, öğrenme gerekçelerini, farklı paradigmaları ve kitabın çalışma yöntemini açıklar. Önsöz ve Giriş birinci düzeyde, Giriş alt başlıkları ikinci ve gerekirse üçüncü düzeyde içindekilere katılır. Bu ön bölümler problem kimliklerini ve tam 100 soru sayımını değiştirmez. Kaynak metin değiştirildiğinde normal derleme aynı Markdown dosyalarından yeni Word belgesini üretir; Word içeriğini elle kalıcı olarak değiştirmeyin.

En zor son grubun soru seçimi ve kapsamı [091–100 planında](plans/091-100.md) yer alır. Her problem kendi girdi/çıktı sözleşmesini, algoritmasını, tam C# çözümünü, örneklerini, sınır durumlarını, kazanımlarını ve alıştırmalarını içerir.

[assets/back-cover.png](assets/back-cover.png) bütün problemlerin ardından kitabın son sayfasına arka kapak olarak eklenir. Ön kapak gibi ayrı A4 kesitte, sıfır kenar boşluklarıyla tam sayfa yerleştirilir. Arka kapağın altbilgisi boştur; önceki bölümün sayfa numarası bu sayfaya taşınmaz. Görsel değişiklikleri yeniden derlemede yüklenir.

## İlk 10 problem

| Sıra | Problem | Yeni odak |
| --- | --- | --- |
| 001 | [İki sayının toplamı](problems/001-iki-sayinin-toplami.md) | Girdi, değişken, çıktı ve toplama |
| 002 | [Dikdörtgenin alanı ve çevresi](problems/002-dikdortgenin-alani-ve-cevresi.md) | Formüller ve ondalıklı hesaplama |
| 003 | [Celsius Fahrenheit dönüşümü](problems/003-celsius-fahrenheit-donusumu.md) | İşlem önceliği ve birim dönüşümü |
| 004 | [İki değişkenin değerini takas etme](problems/004-iki-degiskenin-degerini-takas-etme.md) | Atama sırası ve geçici değişken |
| 005 | [Sayının işaretini belirleme](problems/005-sayinin-isaretini-belirleme.md) | Birden fazla koşul |
| 006 | [Çift tek sayı](problems/006-cift-tek-sayi.md) | Kalan operatörü ve iki seçenek |
| 007 | [Üç sayıdan en büyüğü](problems/007-uc-sayidan-en-buyugu.md) | Karşılaştırma ve aday güncelleme |
| 008 | [Birden N ye toplam](problems/008-birden-nye-toplam.md) | Sayaç ve biriktirici döngüsü |
| 009 | [Faktöriyel hesaplama](problems/009-faktoriyel-hesaplama.md) | Çarpım döngüsü ve taşma sınırı |
| 010 | [Dizide toplam ve ortalama](problems/010-dizide-toplam-ve-ortalama.md) | Dizi gezme ve tür dönüşümü |

İlk derste girdi doğrulama kalıbını hazır bir yardımcı işlem olarak ele alın. Önce problemdeki temel hesabı izleyin; ardından `TryParse`, başarısız girdi ve erken sonlandırmayı inceleyin. Örnekler bağımsız konsol programlarıdır; kullanıcı tanımlı sınıf tasarımı gerektirmez.

## İkinci 10 problem

Bu grup, tek bir koşulla karar vermekten birden fazla kuralı birlikte uygulamaya ilerler. Zorluk, satır sayısından çok koşulların ilişkisi ve sınır durumlarının sayısıyla artar. 018 bölme ve kalanla zaman dönüşümünü pekiştirir; 019 ve 020 önceki karar kurallarını takvim problemlerinde birleştirir.

| Sıra | Problem | Yeni odak |
| --- | --- | --- |
| 011 | [Sayının kapalı bir aralıkta olması](problems/011-sayinin-aralikta-olmasi.md) | İki sınırı `&&` ile birlikte denetleme |
| 012 | [Puanı harf notuna dönüştürme](problems/012-puani-harf-notuna-donusturme.md) | Sıralı koşullar ve kesintisiz aralıklar |
| 013 | [Atölyeye katılım uygunluğu](problems/013-atolyeye-katilim-uygunlugu.md) | `&&`, `||` ve parantezlerle birleşik koşul |
| 014 | [Artık yılı belirleme](problems/014-artik-yili-belirleme.md) | Genel kural ve istisnaları birleştirme |
| 015 | [Dört işlem hesap makinesi](problems/015-dort-islem-hesap-makinesi.md) | `switch`, geçersiz seçim, sıfıra bölme ve taşma |
| 016 | [Üçgenin geçerliliği ve türü](problems/016-ucgenin-gecerliligi-ve-turu.md) | Geçerlilik denetiminden sonra sınıflandırma |
| 017 | [Kademeli tüketim bedeli](problems/017-kademeli-tuketim-bedeli.md) | Kademelerdeki miktarları ayrı hesaplama |
| 018 | [Saate saniye ekleme](problems/018-saate-saniye-ekleme.md) | Birim dönüşümü, bölüm ve kalan, gün geçişi |
| 019 | [Bir ayın gün sayısı](problems/019-ayin-gun-sayisi.md) | Ay seçimiyle artık yıl kuralını birleştirme |
| 020 | [Bir tarihin ertesi günü](problems/020-tarihin-ertesi-gunu.md) | Gün, ay ve yıl sınırlarında durum güncelleme |

Puan aralıkları, atölye koşulları ve tüketim tarifesi bu dersler için problem içinde tanımlanır. Takvim problemleri aynı artık yıl kuralını kullanır. Her dosya önkoşullarını belirtir; öğrencinin çözümü çalıştırmadan önce karar tablosunu ve sınır örneklerini incelemesi önerilir.

## Üçüncü 10 problem

Bu grup, koşulları tekrar eden işlemlerin içinde kullanır. Önce bitiş değerine bağlı bir `while` döngüsü, ardından en az bir kez çalışan `do / while` döngüsü ele alınır. Basamak problemleri değişen bir sayı üzerinde çalışmayı öğretir. Sonraki örneklerde birbirine bağlı ara değerler, erken sonlandırma ve her adımda küçülen bir problem kullanılır.

| Sıra | Problem | Yeni odak |
| --- | --- | --- |
| 021 | [Bitiş değeriyle toplam ve ortalama](problems/021-bitis-degeriyle-toplam-ve-ortalama.md) | `while`, bitiş değeri ve boş veri kümesi |
| 022 | [Basamak sayısı ve toplamı](problems/022-basamak-sayisi-ve-toplami.md) | `do / while`, son basamağı ayırma ve sıfır girdisi |
| 023 | [Basamakları ters çevirme](problems/023-sayinin-basamaklarini-ters-cevirme.md) | Basamaklardan yeni sayı oluşturma ve `long` sonuç |
| 024 | [Palindrom sayı denetimi](problems/024-palindrom-sayi.md) | Özgün sayıyı koruma ve ters sayı ile karşılaştırma |
| 025 | [Tamsayı kuvveti](problems/025-tamsayi-kuvveti.md) | Tekrarlı çarpma, boş çarpım ve tür sınırları |
| 026 | [Fibonacci dizisinin N numaralı terimi](problems/026-fibonacci-sayisi.md) | Önceki iki terimi doğru sırayla güncelleme |
| 027 | [Asal sayı denetimi](problems/027-asal-sayi-denetimi.md) | İlk böleni bulunca durma ve taşmadan bölen sınırı kurma |
| 028 | [Öklid algoritmasıyla EBOB](problems/028-oklid-algoritmasiyla-ebob.md) | Kalanla problemi küçültme ve sonlanma gerekçesi |
| 029 | [EBOB yardımıyla EKOK](problems/029-ebob-yardimiyla-ekok.md) | Önceki algoritmayı birleştirme ve bölmeden sonra çarpma |
| 030 | [Asal çarpanlara ayırma](problems/030-asal-carpanlara-ayirma.md) | Değişen kalan sayıya bağlı döngü sınırı |

021–026 orta düzeyde döngü ve basamak çalışmalarıdır. 027–030 ileri düzeye geçişte işlem sınırını daraltma, sonlanmayı açıklama ve güvenli tamsayı hesabı üzerinde durur. Girdi sınırları her problemde belirtilir. Hazır kuvvet, ters çevirme veya asal çarpan bulma araçları yerine algoritmanın adımları C# kodunda görünür tutulur.

## Dördüncü 10 problem

Bu grup, bir döngünün her turunda başka bir döngüyü yeniden başlatmayı öğretir. İlk beş problemde satır, sütun, boşluk ve yıldız sayıları görünür çıktılarla izlenir. Sonraki problemler aynı yapıyı katsayı üretimi, sayı sınıflandırma ve kısıtlı seçenekleri araştırmak için kullanır. Zorluk, birden fazla sayacın ilişkisini ve aday başına yeniden başlatılması gereken ara değerleri yönetmekle artar.

| Sıra | Problem | Yeni odak |
| --- | --- | --- |
| 031 | [N × N çarpım tablosu](problems/031-nxn-carpim-tablosu.md) | Satır ve sütun döngüleri, iç sayacın yeniden başlaması |
| 032 | [Yıldızlarla dik üçgen](problems/032-yildizlarla-dik-ucgen.md) | Satıra bağlı iç döngü sınırı |
| 033 | [Floyd sayı üçgeni](problems/033-floyd-sayi-ucgeni.md) | Satırlar arasında devam eden ayrı sayaç |
| 034 | [Ortalanmış yıldız piramidi](problems/034-ortalanmis-yildiz-piramidi.md) | Boşluk ve yıldız döngülerinin ilişkisi |
| 035 | [İçi boş dikdörtgen](problems/035-ici-bos-dikdortgen.md) | Konuma bağlı sınır koşulları |
| 036 | [Pascal üçgeni](problems/036-pascal-ucgeni.md) | Satır başında sıfırlanan katsayı ve yinelemeli hesap |
| 037 | [Bir aralıktaki asal sayılar](problems/037-araliktaki-asal-sayilar.md) | Her aday için bölen aramasını yeniden başlatma |
| 038 | [Mükemmel sayılar](problems/038-mukemmel-sayilar.md) | Bölen çiftleri ve aday başına biriktirme |
| 039 | [Pisagor üçlüleri](problems/039-pisagor-ucluleri.md) | Üç iç içe döngü ve sıralı adaylarla tekrarları önleme |
| 040 | [Paralarla tutar oluşturma](problems/040-paralarla-tutar-olusturma.md) | İki seçimi gezip üçüncüyü kısıttan hesaplama |

Bütün problemler ileri düzeydedir; grup içinde küçük adımlarla ilerlenir. Desenlerin tam çıktıları `text` bloklarında verilir; baştaki boşluklar bu örneklerin parçasıdır. Büyük çıktılar için sınır denetimleri ayrıca belirtilir. Öğrenciden dış döngünün görevini, iç döngünün ne zaman yeniden başladığını ve her çözümün neden yalnız bir kez üretildiğini açıklaması beklenir.

## Beşinci 10 problem

Bu grup, iç içe döngülerde öğrenilen sayaç ilişkilerini saklanan veriye uygular. 010'da tanıtılan dizi yapısı artık arama, dönüştürme, özetleme ve tekrarsız sonuç üretmek için kullanılır. Dizinler sıfırdan başlar; aralıkların uçları ve eşit değerlerde ilk geçişin korunması her problemde açıkça tanımlanır.

| Sıra | Problem | Yeni odak |
| --- | --- | --- |
| 041 | [Dizinin en küçük ve en büyük değerleri](problems/041-dizide-en-kucuk-ve-en-buyuk.md) | Değerle birlikte ilk dizinini koruma |
| 042 | [Doğrusal arama](problems/042-dizide-dogrusal-arama.md) | İlk eşleşmede durma ve bulunamama durumu |
| 043 | [Diziyi yerinde ters çevirme](problems/043-diziyi-yerinde-ters-cevirme.md) | İki uçtan ilerleme ve güvenli takas |
| 044 | [Diziyi sağa döndürme](problems/044-diziyi-saga-dondurme.md) | Kalanla adım sayısını azaltma ve geriye kopyalama |
| 045 | [Ön ek toplamıyla aralık toplamı](problems/045-on-ek-toplamiyla-aralik-toplami.md) | Ek dizi, dahil uçlar ve `long` toplam |
| 046 | [Azalmayan sırayı denetleme](problems/046-dizinin-sirasini-denetleme.md) | Komşu elemanlar ve ilk ihlal |
| 047 | [İkili arama ile ilk eşleşme](problems/047-dizide-ikili-arama.md) | Sıralılık önkoşulu ve arama aralığını daraltma |
| 048 | [Değerlerin frekansları](problems/048-dizide-frekans-hesaplama.md) | Değeri dizi dizinine dönüştürme |
| 049 | [İki dizinin ortak değerleri](problems/049-iki-dizinin-ortak-degerleri.md) | Tekrarsız kesişim ve ilk dizinin sırasını koruma |
| 050 | [Tekrarları yerinde kaldırma](problems/050-dizide-tekrarlari-yerinde-kaldirma.md) | Okuma konumu, yazma konumu ve geçerli veri bölgesi |

Bütün problemler ileri düzeydedir. Elemanları okumakla sonuç üretmek ayrı aşamalardır; hatalı veya eksik veri için kısmi sonuç yazılmaz. Yerinde çalışan algoritmalarda işlenmemiş hücrelerin neden korunabildiği açıklanır. Arama ve küme işlemlerinin adımları C# kodunda görünür tutulur. Öğrencinin dizi uzunluğu ile geçerli eleman sayısını ayırt etmesi, sonraki sıralama grubuna hazırlık sağlar.

## Altıncı 10 problem

Bu grup, veriyi taramaktan sırasını sistematik olarak değiştirmeye geçer. İlk üç problem aynı sıralama amacına farklı yollarla ulaşır; karşılaştırma, takas ve kaydırma adımlarının tanımları açıkça verilir. Sonraki problemler değer aralığını ve sıralı bölümleri kullanarak birleştirme, ekleme, sıra seçimi ve istatistik üretir.

| Sıra | Problem | Yeni odak |
| --- | --- | --- |
| 051 | [Seçmeli sıralama](problems/051-secmeli-siralama.md) | Kalan bölümün minimumu, kesinleşen başlangıç ve gerçek takas |
| 052 | [Erken bitişli kabarcık sıralama](problems/052-kabarcik-siralama.md) | Kesinleşen son bölüm ve değişmeyen turda durma |
| 053 | [Eklemeli sıralama](problems/053-eklemeli-siralama.md) | Anahtarı koruma ve sıralı bölüm içinde geriye kaydırma |
| 054 | [Sayarak sıralama](problems/054-sayarak-siralama.md) | Frekans dizisiyle sınırlı değer aralığını sıralama |
| 055 | [İki sıralı diziyi birleştirme](problems/055-sirali-dizileri-birlestirme.md) | İki okuma konumu ve kalan elemanları tamamlama |
| 056 | [Sıralı diziye eleman ekleme](problems/056-sirali-diziye-eleman-ekleme.md) | Boş kapasite, eşitlerden sonra ekleme ve geriye kopyalama |
| 057 | [Dizinin medyanını bulma](problems/057-dizinin-medyanini-bulma.md) | Sıralı verinin ortası, tek/çift uzunluk ve güvenli ara toplam |
| 058 | [K numaralı farklı küçük değer](problems/058-k-nci-farkli-kucuk-deger.md) | Eşit komşuları atlayarak farklı değerlerin sırasını sayma |
| 059 | [Dizinin modunu sıralama ile bulma](problems/059-dizinin-modunu-bulma.md) | Ardışık eşit değer grupları ve eşit sıklıkta küçük değeri seçme |
| 060 | [İki sıralama algoritmasını karşılaştırma](problems/060-siralama-algoritmalarini-karsilastirma.md) | Aynı verinin ayrı kopyalarında aynı ölçüyü kullanma |

Bütün problemler ileri düzeydedir. Hazır sıralama ve kopyalama araçları yerine C# döngüleri kullanılır. Sıralı girdi isteyen 055 ve 056 bu önkoşulu doğrular; 057–059 girdiyi kendileri sıralar. 060, çalışma süresi yerine değer karşılaştırmalarını ölçer; takas sayısı ayrıca verilir. Tek bir ölçümün veya örneğin bütün girdiler için üstünlük göstermediği açıklanır. Her dosyada küçük tam çıktılar, izleme tabloları, sınır durumları ve alıştırmalar bulunur.

## Yedinci 10 problem

Bu grup, dizilerde öğrenilen tarama ve sınır yönetimini karakter verisine taşır. Zorluk, karakterleri saymaktan sözcük sınırlarını ve iki uçtan taramayı yönetmeye, ardından örtüşen arama, kodlama, sayı ayrıştırma ve yığınla iç içe eşleştirmeye ilerler.

| Sıra | Problem | Yeni odak |
| --- | --- | --- |
| 061 | [Karakter sınıflandırma ve harf frekansı](problems/061-karakter-siniflandirma-ve-harf-frekansi.md) | Karakter aralıkları, harf normalleştirme ve frekans eşitliği |
| 062 | [Boşlukları normalleştirme ve sözcük sayma](problems/062-bosluklari-normallestirme.md) | Sözcük başlangıcı, ertelenen boşluk ve geçerli tampon uzunluğu |
| 063 | [En uzun sözcüğü ve başlangıç dizinini bulma](problems/063-en-uzun-sozcugu-bulma.md) | Sözcük sınırları, özgün konum ve ilk eşit aday |
| 064 | [Metinde iki uçtan palindrom denetimi](problems/064-metinde-palindrom-denetimi.md) | Karakter atlama, yerel normalleştirme ve erken sonlandırma |
| 065 | [Harf frekanslarıyla anagram denetimi](problems/065-anagram-denetimi.md) | İki metnin harf adetlerini tek fark dizisinde karşılaştırma |
| 066 | [Örtüşen alt metin eşleşmelerini bulma](problems/066-ortusen-alt-metin-aramasi.md) | Aday başlangıç, iç karşılaştırma ve örtüşen sonuçlar |
| 067 | [Ardışık harfleri adetleriyle kodlama](problems/067-ardisik-harfleri-sikistirma.md) | Grup sınırı, çok basamaklı adet ve kod uzunluğu |
| 068 | [Metni elle tamsayıya dönüştürme](problems/068-metni-elle-tamsayiya-donusturme.md) | İşaret/rakam dilbilgisi ve işlem öncesi taşma denetimi |
| 069 | [Harf ve adet kodunu doğrulayarak açma](problems/069-sikistirilmis-metni-acma.md) | Değişken uzunluklu alanlar, toplam kapasite ve kısmi çıktıyı önleme |
| 070 | [Yığınla farklı parantez türlerini eşleştirme](problems/070-yiginla-parantez-eslestirme.md) | Diziyle yığın, son giren ilk çıkar ve ilk hata konumu |

Her problem kendi karakter kümesini ve giriş uzunluğunu tanımlar. Bu gruptaki ASCII kısıtı, kabul edilmiş her karakterin bir `char` ve bir dizin konumu olmasını sağlar; Türkçe harfler veya genel Unicode metinleri desteklendiği varsayılmaz. Açıklamalar ve çıktılar Türkçedir. Boş satır ile EOF ayrılır; giriş hatasında kısmi sonuç yazılmaz. Hazır arama, sıralama, sözcük bölme, gruplama, sayı ayrıştırma veya yığın araçları yerine temel C# döngüleri ve dizileri kullanılır. Her dosyada tam çıktılar, izleme tabloları, sınır durumları ve alıştırmalar bulunur.

## Sekizinci 10 problem

Bu grup, önceki algoritmaları açık metot sözleşmeleriyle birleştirir. Önce parametre ve dönüş değeri, sonra `out` ile başarı/veri aktarımı ve `ref` ile değişiklik ele alınır. Zorluk, birden fazla metot arasında veri akışı kurmaktan özyineleme, böl ve yönet, geri izleme ve bir ifade değerlendiricisine ilerler.

| Sıra | Problem | Yeni odak |
| --- | --- | --- |
| 071 | [Metotlarla aralıktaki asal sayıları özetleme](problems/071-metotlarla-asal-sayi-ozeti.md) | Parametre, dönüş değeri ve konsoldan bağımsız hesaplama |
| 072 | [Metot zinciriyle kesri sadeleştirme](problems/072-metotlarla-kesir-sadelestirme.md) | Metot bileşimi, işaret normalleştirme ve tür sınırları |
| 073 | [Tekrar kullanılabilir giriş metoduyla not raporu](problems/073-metotla-dogrulanan-not-raporu.md) | `out`, Try sözleşmesi, sınırlı yeniden deneme ve EOF |
| 074 | [ref ve dizi parametreleriyle yerinde döndürme](problems/074-metotlarla-diziyi-dondurme.md) | Değer parametresi, dizi referansı ve `ref` farkı |
| 075 | [out parametrelerle tek geçişte dizi özeti](problems/075-out-parametrelerle-dizi-ozeti.md) | Çok sonuçlu sözleşme ve geçerli boş veri |
| 076 | [Metotlarla sözcük frekansı raporu oluşturma](problems/076-metotlarla-sozcuk-frekansi.md) | İşlem hattı, paralel diziler ve iki ölçütlü sıralama |
| 077 | [Özyinelemeli ikili aramayla ilk eşleşmeyi bulma](problems/077-ozyinelemeli-ilk-eslesme-aramasi.md) | Taban durum, küçülen aralık ve ortak çağrı sayacı |
| 078 | [Özyinelemeli birleştirmeli sıralama](problems/078-ozyinelemeli-birlestirmeli-siralama.md) | İki alt çağrı, ortak tampon ve kararlı birleştirme |
| 079 | [Geri izlemeyle hedef toplamlı alt kümeleri sayma](problems/079-geri-izlemeyle-alt-kume-sayma.md) | Seç/dışla dalları, pozitif veriyle budama ve ziyaret ölçümü |
| 080 | [Metotlarla postfix ifade değerlendirme](problems/080-metotlarla-postfix-hesaplayici.md) | Ayrıştırma, yığın, güvenli aritmetik ve hata aktarımı |

Çözümler bağımsız `Program.cs` dosyalarında `static` yerel fonksiyonlar kullanır. Yeni sınıf tasarımı gerekmez; metotların parametreleri, önkoşulları, dönüşleri ve değiştirdikleri veri açıklanır. Konsola erişen giriş metotları ile hesaplama metotları ayrılır. Özyineleme ve geri izleme için sonlanma gerekçeleri ve giriş boyutu sınırları verilir. Her problemde tam çıktılar, izleme tablosu, sınır durumları ve alıştırmalar bulunur.

## Dokuzuncu 10 problem

Bu grup dosya okuma, kayıt doğrulama ve ilişkili verileri birleştirmeyi öğretir. İlk örneklerde satır ve EOF ayrımı kurulur; son örneklerde iki veri kaynağı doğrulanıp mevcut dosyayı ezmeden rapor üretilir.

| Sıra | Problem | Yeni odak |
| --- | --- | --- |
| 081 | [Dosyanın satır envanteri](problems/081-dosyanin-satir-envanteri.md) | UTF-8, fiziksel satır, EOF ve using |
| 082 | [Sensör dosyasında artış serisi](problems/082-sensor-dosyasinda-artis-serisi.md) | Dosya sırasında durum ve satır doğrulama |
| 083 | [Stok dosyasında kayıt doğrulama](problems/083-stok-dosyasinda-kayit-dogrulama.md) | Alan sayısı, benzersiz anahtar ve ondalık tutar |
| 084 | [Tarihli ölçümlerde eksik günler](problems/084-tarihli-olcumlerde-eksik-gunler.md) | Sabit tarih biçimi, sıra ve eksik gözlem |
| 085 | [Dosyada ardışık veri paketleri](problems/085-dosyada-ardisik-veri-paketleri.md) | Başlık/gövde, çoklu veri kümesi ve eksik EOF |
| 086 | [Tırnaklı CSV kayıtlarını çözümleme](problems/086-tirnakli-csv-kayitlarini-cozumleme.md) | Kaçırılmış tırnak ve hata sütunu |
| 087 | [Hesap hareketlerini sırayla denetleme](problems/087-hesap-hareketlerini-sirayla-denetleme.md) | İş kuralı, ara durum ve benzersiz hareket |
| 088 | [İki sayım dosyasını uzlaştırma](problems/088-iki-sayim-dosyasini-uzlastirma.md) | İki kaynak, ortak ve tek taraflı kayıt |
| 089 | [Katalog ve satışlardan rapor](problems/089-katalog-ve-satislardan-rapor.md) | Referans, birikim, paralel sıralama ve yeni çıktı |
| 090 | [İş ağacında birikimli süre raporu](problems/090-is-agacinda-birikimli-sure-raporu.md) | Üst ilişkisi, döngü denetimi ve özyinelemeli rapor |

Her dosyada tam veri biçimi, örnek dosyalar, tam çıktılar ve izleme tablosu bulunur. Ortak küçük dosya sınırları bellek kullanımını sınırlar; bozuk kodlama, eksik dosya, boş veri, anahtar tekrarı ve hata konumu açıklanır. Sayı ve tarihler kültürden bağımsızdır. Dosya testleri ayrılmış geçici verilerde yürütülür. 091–100 grubu bu becerileri birleşik uygulamalarda kullanır.

## Onuncu 10 problem

Son grup ilişkili kayıtları, zaman aralıklarını ve paylaşılan kaynakları birleştirir; ardından dinamik programlama, çizge ve kısıtlı optimizasyonla tamamlanır. Yeni tekniklerin durumları, sonlanma gerekçeleri ve eşitlik kuralları ilgili problemde açıklanır.

| Sıra | Problem | Yeni odak |
| --- | --- | --- |
| 091 | [Ders denemelerinden ağırlıklı başarı](problems/091-ders-denemelerinden-agirlikli-basari.md) | En iyi deneme, bileşik anahtar, kredi ağırlığı |
| 092 | [Salon rezervasyonlarında çakışma taraması](problems/092-salon-rezervasyonlarinda-cakisma-taramasi.md) | Yarı açık aralıklar, tüm çakışan çiftler |
| 093 | [Ödünç ve iadelerden gecikme raporu](problems/093-odunc-ve-iadelerden-gecikme-raporu.md) | Kesim tarihindeki durum, çoklu ilişkiler |
| 094 | [Depolar arası transfer ve stok korunumu](problems/094-depolar-arasi-transfer-ve-stok-korunumu.md) | Stok matrisi, ara kontroller, korunum |
| 095 | [Reçete ve siparişlerden üretim planı](problems/095-recete-ve-siparislerden-uretim-plani.md) | Öncelik, ortak kaynak, tam sipariş kabulü |
| 096 | [Vardiyaları ücret dilimlerine bölme](problems/096-vardiyalari-ucret-dilimlerine-bolme.md) | Gece yarısı, aralık parçalama, maliyet |
| 097 | [Bütçe kısıtlı proje seçimi](problems/097-butce-kisitli-proje-secimi.md) | 0/1 dinamik programlama, seçim geri kurma |
| 098 | [En düşük maliyetli rota](problems/098-en-dusuk-maliyetli-rota.md) | Dijkstra, rota ve eşitlik kuralları |
| 099 | [Kritik yol ve zaman payı](problems/099-kritik-yol-ve-zaman-payi.md) | Topolojik sıra, erken/geç zamanlar |
| 100 | [En iyi görev ataması](problems/100-en-iyi-gorev-atamasi.md) | Geri izleme, iyimser alt sınır, optimum |

Son 10 çözüm, gerçek Markdown C# bloklarından alınan bağımsız .NET 10 projelerinde derlenmiştir. 1455 çalışma kontrolü; örnekler, sınırlar, geçersiz girdiler ve bağımsız referans sonuçlarını kapsar. Proje seçimi için tüm alt kümeler, rota için tüm basit yollar, zaman hesabı için tüm DAG yolları ve görev ataması için budamasız tüm permütasyonlar küçük girdilerde karşılaştırılmıştır. Girişlerin korunması, kaynakların kapanması ve raporun UTF-8/LF baytları da denetlenmiştir. Kontrol kanıtları `build/tenth-group-check/` altındadır.

## Klasör düzeni

```text
procedural-programming/
  problems/              # Her problem için bir Markdown dosyası
  front-matter/01-onsoz.md # Önsöz; problem sayımından ayrı
  front-matter/02-giris.md # Giriş, alt başlıklar, tablo ve kavram örnekleri
  chapters.md            # Onar soruluk bölüm başlıkları
  templates/problem.md   # Yeni problem şablonu
  assets/figures/         # Markdown diagram bloklarından üretilen PNG önizlemeleri
  tools/WorkbookBuilder/ # C# konsol uygulaması: doğrulama, şekiller ve Word üretimi
  tests/WorkbookBuilder.Tests/ # C# ile derleme davranışı kontrolleri
  build.ps1              # C# uygulamasını çağıran isteğe bağlı kısa komut
  ROADMAP.md             # 100 probleme doğru konu aşamaları
  build/                 # Üretilen Word dosyası ve isteğe bağlı PDF; Git dışında
```

## Kurulum ve derleme

Windows ve .NET 10 SDK gerekir. Sayfa numaralarını, içindekileri ve şekil dizinini derleme sırasında güncellemek için masaüstü Microsoft Word gerekir. Problem çözümleri, Markdown doğrulaması, şekil üretimi, Word üretimi ve testler C# ile yazılmıştır. C# örnekleri repodaki diğer projelerle aynı şekilde .NET 10 konsol uygulamalarında çalışır.

Repo kökünde çalıştırın:

```powershell
dotnet run --project procedural-programming/tools/WorkbookBuilder
```

İlk çalıştırmada NuGet bağımlılıkları otomatik yüklenir. Her Markdown değişikliğinden sonra aynı komutu çalıştırın. İsterseniz kısa PowerShell komutunu da kullanabilirsiniz:

```powershell
./procedural-programming/build.ps1
```

Çıktı `procedural-programming/build/prosedurel-programlama.docx` dosyasıdır. C# uygulaması Word'ü görünmez bir otomasyon oturumunda açar; içindekiler, şekil numaraları, şekil dizini ve sayfa alanlarını güncelleyip belgeyi kaydeder. Belge açıkken derleme yapmadan önce kapatın. `build.ps1` yalnızca bu C# uygulamasını çağırır.

Yalnız kaynak kontrolü ve isteğe bağlı PDF:

```powershell
dotnet run --project procedural-programming/tools/WorkbookBuilder -- --check
dotnet run --project procedural-programming/tools/WorkbookBuilder -- --pdf
```

Microsoft Word kurulu olmayan bir Windows bilgisayarda:

```powershell
dotnet run --project procedural-programming/tools/WorkbookBuilder -- --skip-word-fields
```

Bu adım şekilleri ve tıklanabilir dizinleri üretir. Sayfa numaraları için belgeyi masaüstü Word'de açıp `Ctrl+A`, `F9` ile bütün alanları güncelleyin; içindekiler için istendiğinde tüm tabloyu güncelleme seçeneğini kullanın. PowerShell sarmalayıcısında aynı davranış `-SkipWordFields` seçeneğiyle alınabilir. Word alan güncellemesi atlandığında sayfa numaraları otomatik hesaplanmaz. `--root` ile kaynak klasörü, `--output` ile çıktı dosyası değiştirilebilir; `--help` seçenekleri listeler.

## C# derleyici projesi

`tools/WorkbookBuilder` aşağıdaki görevleri ayrı dosyalarda düzenler:

- `LessonLoader.cs`: YAML üst bilgisini ve Markdown yapısını doğrular; önkoşulları ve problem sırasını kontrol eder.
- `FrontMatterLoader.cs`: isteğe bağlı Önsöz ve Giriş kaynaklarını problem koleksiyonundan ayrı yükleyip doğrular. Önsöz metin paragrafları içerir; Girişte ikinci/üçüncü düzey başlıklar, metin, tablolar, listeler, küçük `csharp`/`text` blokları ve HTTPS kaynak bağlantıları desteklenir. Geçersiz ön bölüm, mevcut çıktı değiştirilmeden reddedilir.
- `ChapterLoader.cs`: `chapters.md` başlıklarını yükler; her bölümün on problemden oluştuğunu, kimliklerin ve öğretim sırasının bölüm aralıklarıyla eşleştiğini doğrular.
- `DiagramRenderer.cs`: Markdown içindeki JSON akış şemalarını PNG resmine dönüştürür.
- `WordBookWriter.cs`: başlıklar, tablolar, C# kodları, resimler ve dizin alanlarıyla Word belgesini oluşturur.
- `WordFields.cs`: Microsoft Word üzerinden alanları ve sayfa numaralarını günceller.
- `Program.cs`: konsol seçeneklerini ve derleme akışını yönetir.

Markdown için Markdig, YAML için YamlDotNet, Word için Open XML SDK ve şekiller için System.Drawing kullanılır. Paket sürümleri proje dosyasında sabitlenmiştir. Şekil üretimi Windows üzerinde çalışır.

## Yeni problem ekleme

1. [Şablonu](templates/problem.md) öğretim amaçlı yeni bir çalışma için kopyalayın. Bu kitap 001–100 aralığıyla tamamlanmıştır; mevcut kitabı güncellerken ilgili dosyayı düzenleyin. Yeni problem eklemek toplam 100 soru kapsamını değiştirir.
2. `id` alanını üç basamaklı, tırnak içinde bir metin olarak yazın. `order` öğretim sırasını belirler; dosya adı `id` ile başlamalıdır. Kimlik ve sıra benzersiz olmalıdır.
3. Başlığı, önkoşulları ve kavramları güncelleyin. Önkoşullar daha önce gelen mevcut problem kimlikleri olmalıdır.
4. Sekiz bölümün tamamını doldurun. Her dosyada yalnız bir tam `csharp` kod bloğu bulunmalıdır. Girdi aralıklarını ve sınır durumlarını açıklayın; örnek sonuçları kodla karşılaştırın.
5. Kaynakları kontrol edin ve Word belgesini yeniden derleyin. Derleyici `problems/*.md` dosyalarını otomatik bulur; ayrı bir problem listesi düzenlemek gerekmez.

Derleyici paragrafları, başlıkları, sıralı ve sırasız listeleri, kalın/italik metni, satır içi kodu, bağlantıları, tabloları, yerel PNG/JPEG resimleri ve `csharp`, `text`, `diagram` bloklarını destekler. Tablo hücresinde birden fazla çıktı satırı için `<br>` kullanılabilir. Diğer HTML ve Mermaid doğrudan desteklenmez.

## Şekillerin yönetimi

Şekiller gerekli oldukları problemlerde kullanılır. 004, 005, 008, 014, 020, 021 ve 028 örneklerinde atama, koşul, döngü, takvim ve Öklid algoritması akışları Markdown içindeki `diagram` JSON bloğuyla tanımlıdır. Bu bloklar her derlemede PNG resmine dönüştürülür ve Word'e açıklamasıyla eklenir. Hemen ardından gelen aynı PNG bağlantısı Markdown önizlemesini sağlar; Word'e ikinci kez eklenmez. JSON bloğunu değiştirdikten sonra derleyip güncel PNG'yi de Git'e ekleyin.

Şema düğümleri `id`, `text`, `kind`, `x`, `y`; oklar `from`, `to`, isteğe bağlı `label`, `via` ve `label_at` alanlarını kullanır. Düğüm türleri `terminal`, `process`, `decision`, `io` değerleridir. Koordinatlar ızgarada düğüm merkezlerini belirtir; `via` okların ara geçiş noktalarıdır. Otomatik etiket konumu dar alanlarda uygun değilse `label_at: [x, y]` ile konum verilebilir. Geniş veya uzun şemaları küçük parçalara ayırarak okunabilirliği koruyun. Şemalar geçerli girdiler için ana algoritmayı gösterir; doğrulama ayrıntıları kodda ve sınır durumlarında açıklanır.

Dışarıdan bir resim eklemek için `![Açıklama](../assets/resim.png)` biçimini kullanın. Resim `procedural-programming` altında bulunmalıdır. Açıklama Word'de şekil başlığına dönüşür. Şekil numaraları ve dizini derleme sırasına göre yeniden oluşturulur.

## Kontroller

```powershell
dotnet test procedural-programming/tests/WorkbookBuilder.Tests
```

C# kodunu çalıştırmak için ayrı bir konsol projesi oluşturup istediğiniz problemin kod bloğunu `Program.cs` içine kopyalayın. Her problem için kendi örnek ve sınır girdilerini deneyin. İçerik ilerledikçe [yol haritasındaki](ROADMAP.md) bir sonraki küçük grubu seçin; 100 dosyayı boş yer tutucularla doldurmayın.

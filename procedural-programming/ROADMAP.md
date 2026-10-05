# 100 probleme doğru öğrenme akışı

001–100 aralığındaki 100 çözümlü problem on grup halinde hazırlanmıştır. Son grubun başlıkları ve kapsamları [091–100 soru planında](plans/091-100.md), çalışabilir C# çözümleri ise ilgili Markdown dosyalarında yer alır. Son 10 çözüm bağımsız derleme ve 1455 çalışma kontrolünden geçmiştir.

| Aralık | Konu odağı | Grubun sonunda beklenen beceri |
| --- | --- | --- |
| 001–010 | İşlem sırası, değişken, koşul, temel döngü ve dizi | Küçük bir problemi girdi, işlem ve çıktı olarak modelleme |
| 011–020 | Birleşik koşullar, karar tabloları ve kuralları birleştirme | Geçerli girdiyi sınıflandırma ve zaman ile takvim sınırlarını yönetme |
| 021–030 | Döngüler, basamak işlemleri ve sayı algoritmaları | Döngü sınırını, ara değerleri ve sonlanma gerekçesini doğru belirleme |
| 031–040 | İç içe döngüler, desenler ve kısıtlı arama | Sayaç ilişkilerini koruyarak her adayı ve çözümü bir kez üretme |
| 041–050 | Diziler, arama, dönüştürme ve özetleme | Dizi sınırlarını ve saklanan veriyi koruyarak sonuç üretme |
| 051–060 | Sıralama ve temel algoritma karşılaştırmaları | Adım adım sıralama ve işlem sayısı karşılaştırması |
| 061–070 | Metin, karakter işleme ve temel ayrıştırma | Karakter sınırlarını, kodlama kurallarını ve iç içe eşleşmeleri yönetme |
| 071–080 | Metotlar, prosedürel ayrıştırma ve özyineleme | Sözleşmeli alt görevleri birleştirme, küçülen alt problem ve seçim dalları kurma |
| 081–090 | Dosyalar ve birden çok veri kümesi | Kalıcı veriyi okuyup doğrulayarak işleme |
| 091–100 | Birleşik uygulamalar ve nesneye yönelik programlamaya hazırlık | Tekrarlanan veri ve davranışları fark edip sınıf tasarımına gerekçe oluşturma |

## İkinci grubun öğretim sırası

011 aralık denetimi ve 012 puan sınıflandırması, karşılaştırmaları sistematik hale getirir. 013 birleşik koşulları ve parantezlerin anlamını ekler; 014 aynı yaklaşımı istisnalı artık yıl kuralında kullanır. 015 seçim için `switch` yapısını tanıtır. 016 geçerlilik denetimini sınıflandırmadan ayırır; 017 aralıkların hesaplamaya etkisini kademeli tarifeyle gösterir. 018 saat bilgisini tek bir birime dönüştürür. 019 ay seçimi ile artık yılı birleştirir; 020 bütün bu takvim kurallarını geçerli bir tarihin ertesi gününü bulmak için kullanır.

## Üçüncü grubun öğretim sırası

021 bitiş değeriyle veri okurken döngünün sonlanmasını ve boş veri durumunu ele alır. 022 son basamağı bölüm ve kalanla ayırır; sıfır için en az bir yineleme gerektiğini gösterir. 023 aynı basamakları yeni bir sayıda biriktirir; 024 özgün değeri koruyarak palindrom denetimi yapar. 025 tekrarlı çarpmayı kuvvet hesabına uygular. 026 önceki iki Fibonacci terimini güncellerken atama sırasının önemini pekiştirir.

027 asal sayı denetiminde ilk böleni bulunca durur ve bölen sınırını çarpma taşması oluşturmadan belirler. 028 Öklid algoritmasında kalanla problemi küçültür. 029 bu algoritmanın sonucunu EKOK hesabında kullanır; tür dönüşümü ve işlem sırası birlikte tartışılır. 030 asal çarpanları bulurken kalan sayı küçüldükçe döngü sınırının değiştiğini gösterir. Bu grupta her döngünün hangi değer nedeniyle sona erdiği öğrenci tarafından açıklanmalıdır.

## Dördüncü grubun öğretim sırası

031 çarpım tablosunda satır ve sütun sayaçlarını ayırır; iç döngünün her satır için yeniden başlamasını görünür kılar. 032 dik üçgende iç döngünün sınırını satır numarasına bağlar. 033 Floyd üçgeninde sütun sayacı yeniden başlarken sayı sayacının devam etmesi gerektiğini gösterir. 034 piramit için boşluk ve yıldız miktarlarını birlikte hesaplar. 035 dikdörtgenin yalnız sınırlarını çizmek için satır ve sütun konumlarını birleşik koşulla denetler.

036 Pascal üçgeninde her satırın ilk katsayısını yeniden başlatır ve sonraki katsayıyı önceki katsayıdan hesaplar. 037 tek sayı için öğrenilen asal denetimini bir aralıktaki her aday için tekrarlar. 038 mükemmel sayı ararken bölenleri çift olarak toplar ve tam karelerin aynı bölenini iki kez saymaz. 039 Pisagor üçlülerini üç döngüyle üretir; `a < b < c` koşulu aynı üçlünün farklı sıralarını önler. 040 iki para adedini gezip üçüncüyü toplam tutar kısıtından hesaplar; böylece üçüncü bir arama döngüsüne ihtiyaç kalmaz.

## Beşinci grubun öğretim sırası

041 en küçük ve en büyük değerlerin ilk dizinlerini birlikte izler. 042 doğrusal aramada ilk eşleşmeyi bulur. 043 iki uçtan ilerleyip elemanları yerinde takas eder. 044 son elemanı saklayarak geriye kopyalar; ham döndürme adedini dizi uzunluğuna göre kalanla azaltır. 045 aralık toplamını dahil uçlarla hesaplamak için uzunluğu bir fazla olan ön ek toplam dizisini kurar.

046 komşu elemanlardan azalmayan sırayı denetler ve ilk ihlali bildirir. 047 bu önkoşulu doğruladıktan sonra ikili arama yapar; eşitlikte soldaki ilk eşleşmeyi aramayı sürdürür. 048 sınırlı değer aralığını frekans dizininin dizinlerine dönüştürür. 049 iki dizinin ortak değerlerini ilk dizinin sırasıyla ve tekrar etmeden üretir. 050 okuma ve yazma konumlarını ayırarak farklı değerleri aynı dizinin başında toplar; fiziksel dizi uzunluğu ile geçerli veri sayısını ayırır.

## Altıncı grubun öğretim sırası

051 seçmeli sıralamada kalan bölümün en küçüğünü bulup başlangıç bölümünü kesinleştirir; yalnızca gerçekten yapılan takaslar sayılır. 052 kabarcık sıralamada komşu değerleri karşılaştırır, her turda son bölümün bir elemanını kesinleştirir ve takas yapılmayan turda durur. 053 eklemeli sıralamada anahtarı saklayıp sıralı başlangıç bölümünde daha büyük değerleri geriye kaydırır. Değer karşılaştırması, dizin denetimi ve veri hareketi birbirinden ayrılır. 054 bilinen dar değer aralığını frekans dizisine dönüştürür; bu kısıtı kullanarak diziyi sayarak sıralar.

055 iki sıralı diziyi baştan ilerleyen iki konumla birleştirir; tekrarları korur ve kalan bölümün tamamını kopyalar. 056 ekleme konumunu eşit değerlerden sonra seçip sağa kaydırır; fiziksel kapasite ile geçerli eleman sayısını ayırır. İki problem de sıralılık önkoşulunu denetler. 057 medyan için tek ve çift uzunluğu ayırır; çift durumda toplamdan önce geniş tür kullanır. 058 sıralanmış veride eşitleri atlayarak K numaralı farklı değeri bulur; yetersiz farklı değer sayısını normal sonuç olarak bildirir. 059 tam `int` aralığındaki veriyi sıralayıp ardışık eşit grupları sayar; eşit sıklıkta en küçük değeri seçer.

060 seçmeli ve erken bitişli kabarcık sıralamayı aynı özgün verinin ayrı kopyalarında çalıştırır. Yalnızca eleman değerleri arasında yapılan karşılaştırmalar ortak ölçüdür; doğrulama ve kopyalama bu sayaca katılmaz. Sıralı, ters sıralı ve tekrarlı örneklerle ölçümün girdiye bağlı olduğu gösterilir. Çalışma süresi veya bütün girdiler için genel üstünlük iddiası yerine sayaçların anlamı ve sınırları açıklanır.

## Yedinci grubun öğretim sırası

061 karakter kod aralıklarını ve 26 elemanlı harf frekansını birlikte kullanır; harf büyüklüğünü normalleştirirken özgün veriyi korur. 062 sözcük başlangıcını bir durum değişkeniyle tanır ve ayırıcı boşluğu sonraki sözcüğe kadar erteler. 063 aynı sınır taramasını en uzun sözcüğü seçmekte kullanır; özgün dizinleri korur ve eşitlikte ilk adayı bırakır. 064 iki uçtan karakter atlayıp yerel normalleştirmeyle palindrom denetler. 065 iki metnin harf adetlerini tek fark dizisinde karşılaştırır; tekrar sayıları korunmadan anagram kararı verilemeyeceğini gösterir.

066 her uygun başlangıçta deseni arar, ilk farkta yalnız o adayı bırakır ve örtüşen sonuçları korur. 067 ardışık harf gruplarını adetleriyle kodlar; kodun her girdide kısa olmadığı ve çok basamaklı adetlerin çıktı uzunluğunu etkilediği incelenir. 068 işaret ve rakam dilbilgisini elle ayrıştırır; taşma denetimini çarpma/toplamadan önce kurar ve negatif `int` sınırının asimetrisini yönetir. 069 bu basamak mantığını harf-adet çiftlerine uygular; alan sınırı ve toplam çıktı kapasitesini ayrı denetler, hata halinde kısmi açılım yazmaz. 070 geçerli eleman adediyle bir dizi yığını kurar; farklı parantez türlerini son giren ilk çıkar kuralıyla eşleştirip ilk hata konumunu raporlar.

Bu grubun metin alanları her dosyada açık ASCII alt kümeleriyle sınırlandırılır. Boş satır, EOF, sıfır anlamlı karakter, hatalı son karakter ve tam kapasite durumları karşılaştırılır. Öğrenci her döngüde hangi dizinin ilerlediğini, hangi hücrelerin geçerli olduğunu ve ne zaman sonuç yazılabileceğini açıklamalıdır.

## Sekizinci grubun öğretim sırası

071 asal denetimini parametre alan ve `bool` döndüren bir metoda ayırır; hesaplama konsol ve dış sayaçlardan bağımsızdır. 072 mutlak değer ile EBOB metotlarını birleştirip işaret ve tür sınırlarını koruyan sade kesir üretir. 073 aynı giriş metodunu öğrenci sayısı ve notlar için kullanır; başarı ile değer `bool` ve `out` yoluyla ayrılır, yeniden deneme ve EOF açıkça yönetilir. 074 diziyi üç ters çevirme ile döndürür; dizi referansının değer olarak aktarılması ile hücre takasının `ref` parametreleri ayırt edilir. 075 tek geçişin çoklu sonuçlarını `out` ile döndürür; boş dizinin tanımsız ölçüleri başarı durumuyla ayrılır.

076 sözcük ayırma, sayma ve sıralamayı bir işlem hattında birleştirir; paralel sözcük/adet dizileri birlikte taşınır. 077 özyinelemeyi taban durum ve küçülen ikili arama aralığıyla tanıtır; eşitlikte ilk dizin aranır. 078 iki alt çağrıyı birleştirmeyle tamamlar; ortak tampon ve kararlılık kuralı açıklanır. 079 seçme/dışlama dallarıyla geri izleme kurar; pozitif veri üzerinde hedef ve kalan toplam budamaları gerekçelendirilir. 080 belirteç ayrıştırma, sayı okuma ve aritmetiği sözleşmeli metotlarda birleştirerek postfix ifade değerlendirir; geçersiz belirteç, eksik işlenen, sıfıra bölme ve taşma farklı hatalardır.

Bu grupta her metot için parametrelerin anlamı, önkoşullar, dönüş değeri ve veri değişiklikleri açıklanır. `static` yerel fonksiyonlar, sınıf tasarlamadan prosedürel modülerliği öğretir. Öğrenci konsol bağımlılığını hesaplamadan ayırmalı, `ref` ile `out` farkını göstermeli ve her özyinelemeli çağrının neden sonlandığını açıklamalıdır. Sayaçlar hangi işlemi ölçtükleriyle tanımlanır; çalışma süresi veya bütün girdiler için üstünlük iddiası kurulmaz.

## Dokuzuncu grubun öğretim sırası

081 fiziksel satırı boş satır ve EOF'tan ayırır; UTF-8, okuma sınırları ve using ile kaynak kapatma öğrenilir. 082 dosya sırasındaki ardışık ölçümlerden artış serisini hesaplar. 083 alan sayısı, sayısal aralık ve benzersiz anahtarı ayrı doğrular; ondalık dilbilgisi ve stok tutarı eklenir. 084 tam tarih biçimini ve kesin artan tarihleri denetleyip eksik gözlem günlerini hesaplar. 085 başlık ve gövdeyi farklı okuma durumları olarak yönetir; her paket için durum sıfırlanır ve eksik EOF gerçek satır konumuyla bildirilir.

086 tırnaklı CSV'nin açıklanmış alt kümesini karakter karakter ayrıştırır; içerikteki virgül, çift tırnak kaçışı ve sütun konumu eklenir. 087 benzersiz hesap hareketlerini dosya sırasında uygulayıp her ara bakiye için iş kuralını denetler. 088 iki sıralı sayım dosyasını ortak ve tek taraflı kayıtları koruyarak uzlaştırır. 089 sırasız katalog ile tekrarlı satışları eşler; paralel dizileri birlikte sıralayıp yeni bir rapor dosyası oluşturur. 090 iş ağacı ve süre dosyasını birleştirir; eksik üst ilişkiyi ve döngüyü denetleyip özyinelemeli birikimli süre üretir. Dizin dizisini sıralamak ilişkilerin yerini korur.

Her problem bağımsızdır ve örnek dosyaların tam içeriğini, yollarını ve beklenen çıktısını belirtir. Boş dosya, boş satır, eksik dosya, hatalı kayıt, anahtar tekrarı, kültürden bağımsız biçim, kapasite ve taşma davranışları açık sözleşmelerdir. Sonuçlar doğrulama bitmeden yazılmaz. Raporlarda CreateNew mevcut dosyayı ezmez; yazım sırasındaki I/O hatasının yeni dosyada kısmi veri bırakabileceği ayrıca açıklanır. Öğretilen arama, eşleme, gruplama, sıralama ve özyineleme hazır LINQ araçlarıyla atlanmaz.

## Onuncu grubun öğretim sırası

| Sıra | Soru | Yeni odak |
| --- | --- | --- |
| 091 | Ders denemelerinden ağırlıklı başarı | Bileşik anahtar, en iyi deneme, ağırlıklı ortalama |
| 092 | Salon rezervasyonlarında çakışma taraması | Yarı açık aralık, bütün çakışan çiftler |
| 093 | Ödünç ve iadelerden gecikme raporu | İlişkili kayıtlar, kesim tarihindeki durum |
| 094 | Depolar arası transfer ve stok korunumu | Stok matrisi, ara adımlar, korunum |
| 095 | Reçete ve siparişlerden üretim planı | Ortak kaynak, öncelik, tam sipariş kabulü |
| 096 | Vardiyaları ücret dilimlerine bölerek maliyet hesaplama | Gece yarısı ve ücret sınırlarında aralık parçalama |
| 097 | Bütçe kısıtlı proje seçimi | 0/1 dinamik programlama, çözümü geri kurma |
| 098 | Yönlü ulaşım ağında en düşük maliyetli rota | Çizge, Dijkstra, eşitlik ve rota geri kurma |
| 099 | Bağımlı işlerde kritik yol ve zaman payı | Topolojik sıra, ileri/geri geçiş, kritik işler |
| 100 | Uygunluk ve maliyetle en iyi görev ataması | Kısıtlı geri izleme, alt sınır, küresel optimum |

091–095 ilişkileri, zaman kesitlerini ve paylaşılan verinin kurallarını birleştirir. 096 zaman aralıklarını parçalayıp süreyi ve maliyeti korur. 097 seçme/dışlama yaklaşımını dinamik programlamaya taşır. 098 çizgede maliyetli yol aramayı; 099 döngüsüz bağımlılık ağında zaman hesaplamayı öğretir. 100, uygunluk kısıtları altında bütün bir atamayı optimize ederek grubu tamamlar. Yeni algoritmaların kavramları ilgili sorularda açıklanır; öğrenciden daha önce öğrenmediği bir tekniği doğrudan uygulaması beklenmez.

Ayrıntılı kapsam ve ayırt edici test durumları [son 10 soru planında](plans/091-100.md), tamamlanmış tanımlar ve çözümler ilgili problem dosyalarında yer alır. Tekrar eden veri ve davranışlar, çözümü sınıf tasarımına dönüştürmeden sonraki nesneye yönelik derste bu tasarımın neden yararlı olacağını kazanım veya alıştırmalarda görünür kılar.

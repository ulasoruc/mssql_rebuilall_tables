# mssql_rebuilall_tables
Girilen database bilgilerine göre tüm tabloların var olan indexlerini sıra ile yapar. / According to the entered database information, it makes the existing indexes of all tables in order.


Ekrandaki Bilgiler doldurulur,
"Rebuild Deneme Sayısı" -> rebuild yaparken bir nedenle hata alırsa kaç kere daha deneyeceğinin bilgisinin girildiği yerdir,
en az "1" girilmelidir.

"Index arası bekleme" -> index rebuild işlemi bittikten sonra bir sonraki index'i rebuild edene kadar bekleyeceği zamanı
saniye cinsinden gösterir.

"Saat" -> hangi saatte başlayacağı buraya girilir (24 H göre)
"Dakika" -> hangi dakikada başlayacağı buraya girilir.


direct exe olarak çalıştırmak için ("...\mssqlRebuildTables\mssqlRebuildTables\bin\Debug\net5.0-windows\mssqlRebuildTables.exe" çalıştırabilirsiniz.

"Başla/Bitir" butonu işlemi kurduğunuz saatte çalıştırmak üzere programı hazırlar.
Programın gidiğiniz (saat ve dakikada) çalışacağını "Durum" alanının "İşlemde" yazması ile anlarız.
Eğer "Durum" alanında "İşlemde" yazmıyor ise (mesala "Durdu" yazıyor is) girilen zamanda rebuildd işlemi yapılmayacaktır.

Yapılan işlemler ekranın altında, log olarak yazılmaktada ve günlük olarak log dosyasına atılmaktadır.

# Mercop
Game: Mercenary doing contracts

2 week sprint game.
3D for android.

30.11.2021
**Krótki opis:**
  Latanie helikopterem, gra 3D, nie jest to symulator lotów tylko w zamyśle dość prosta gra
  Gracz będący najemnikiem stanie przed wyborem kontraktów, które mogą się wzajemnie wykluczać
  Najemnik [czyli gracz] wykonując kolejne kontrakty schodzi na złą ścieżkę lub kroczy dobrą, każda misja oprócz nagród oddziałuje na reputację gracza. Jest to moje rozumienie tematu “Environment changes you” - to z kim współpracuje wpływa na niego i dalszy rozwój wydarzeń
  Najpierw startujemy silnik i wznosimy się, potem można latać przód/tył i obracać się lewym joystickiem
  Gra będzie rozwijana, jest to tylko mały fundament tego co powstanie (bronie, w tym autonaprowadzające, różne helikoptery, opcja zagrania czołgiem, sklep z broniami i skinami, przeciwnicy, consumable typu wezwanie napalm strike’u, niszczenie obiektów itd)
  Level kończymy, lecąc na wprost na drugie lotnisko, lądujemy, startujemy i wracamy do punktu startowego [bazy] i lądujemy - misja ratunkowa, choć póki co nie widać jak na pokład wchodzi człowiek
  Wynik gracza po misji ląduje automatycznie w Leaderboards, gdyż imię wybiera się tuż na starcie gry przed rozpoczęciem kontraktu (w opisie kontraktów znajdują się wstawki z imieniem gracza, stąd musi być wiadome wcześniej)

**Problemy do poprawki (zabrakło czasu na poprawki):**
  Porządek w resourcach po importowaniu darmowych assetów
  Góry na mapie level_1 mają bardzo dużo trójkątów
  Budynki na mapie wywołują sporo drawcalli
  Na Redmi Note 9 gra renderuje się dokładnie połowę wolniej niż powinna (?!), ustawiam targetFramerate 120 to jest 60. Na desktop działa normalnie.
  Dźwięki: lepsze spasowanie i kompresja
  Poatlasowanie wszystkich sprajtów
  Jest zaimplementowany celownik ale nie ma jeszcze broni i strzelania
  Brak ograniczenia widoczności dla lepszej wydajności
  Gdzieniegdzie znajdują się nieużywane pola w klasach lub scriptablach, co wynika z tego że nie wszystko zdążyłem okodować a w przyszłości takie zasoby jak “fuel” będą używane
  Obecnie działa tylko 1 quest z krótkiego opisu. Operacja Grim Dawn służy tylko do prezentacji UI wyboru kontraktów, w rzeczywistości działa tak samo.
  Wymagane drobne poprawki w UI przy scrollbarze wyboru kontraktów
  W kodach znajduje się kilka singletonów - ten w PlayerGuiView jest typowo po to aby wydajniej np. Przesuwać celownik a później np. Aktualizować wyświetlane prędkości lotu, kąt itd., zamiast robienie tego przez EventManagera
  OnValidate są, na pozór zbędnie opaczone dyrektywą if unityeditor, jednak bez tego gra się nie zbuduje na androida
  W kilku miejscach aż się prosi żeby użyć interfejsów zamiast dziedziczenia, jednak na razie jest to zrobione na klasach ze względu na to że interfejsy nie są serializowane i utrudnia to pracę w edytorze. Do przeróbki później (np. Views)
  Brak odrejestrowania się obiektów z danego levelu gry z EventManagera
  Jeszcze możliwe jest w paru miejscach użycie eventów zamiast odwoływanie się do innych obiektów
  Drobny glitch wizualny podczas pierwszego startu helikopterem
  Do poprawy też sterowanie 

**Inne adnotacje:**
  EventSystem po to, aby nie musieć mieć referencji między różnymi klasami w projekcie
  Scena startowa “Common” - zestaw klas i obiektów które i tak byłyby potrzebne w każdym Levelu gry, dlatego levele są doładowywane trybem Additive do sceny Common. Zapobiega to też problemowi z OnDestroyOnLoad.

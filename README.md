# MovieTracker

### Windows Forms Project by: Vojche Stojanoski, Ivona Najdenkoska, Bojan Filipovski

---
### 1.	Oпис на апликацијата
	

Апликацијата MovieTracker која ја развивавме, претставува персонализиран начин на следење филмови кои корисникот ги гледал или планира да ги гледа во иднина. Инспирацијата за имплементација на оваа апликација ја пронајдовме во сопствената потреба за бележење кои филмови сме ги гледале и кои филмови сакаме да ги гледаме. Покрај тоа овозможивме и преглед на детали за било кој филм и преглед на персонални статистики околу гледањето филмови, и тоа се во една единствена апликација.


Сите податоци за филмовите (наслов, режисер, актери, жанр, оценка, времетраење, дејство итн.) се извлечени од [OMDb API (The Open Movie Database)](http://www.omdbapi.com/).

### 2.	Упатство за користење


На почетната страна (табот “Home”) има search bar за пребарување на филмови. Филмовите се пребаруваат според збор од насловот на филмот. Пронајдените филмови се прикажуваат во листата подоле. Со селекција на било кој филм од генерираната листа, се прикажуваат насловот и постерот на филмот. 

![Home page](https://github.com/vojches/MovieTracker/blob/screenshots/screen1.png)


За приказ на деталите за филмот постои посебно копче "Details" кое отвора нов прозорец со сите достапни детали.

![Details](https://github.com/vojches/MovieTracker/blob/screenshots/screen2.png)

Селектираниот филм може да се додаде во една од двете листи: 


    •	Листа со гледани филмови (Watched movies)
    •	Листа со филмови кои корисникот планира да ги гледа (Watchlist). 
    
    
Оваа функционалност е овозможена со кликнување на копчињата за оваа намена.


Освен можноста за преглед на филмови, на левата страна од прозорецот е достапна статистика за избраните филмови од страна на корисникот. Овозможен е преглед на времето поминато во гледање филмови и број на филмови во двете листи, кој се менува во real–time. Има преглед и за рејтингот на гледаните филмови, односно колку филмови со оценка над 9.5  се гледани, колку со оценка од 8.5 - 9.4 итн.

  Во вториот таб “Watched movies” има преглед на филмовите кои корисникот ги гледал. 
  
  
  ![Watched movies](https://github.com/vojches/MovieTracker/blob/screenshots/screen3.png)
  
  
  Филмовите може да се сортираат според два параметри: година на издавање на филмот и оценка (rating). Со кликнување на едно од овие две копчиња, се извршува соодветното сортирање. 
  
  Уште една функционалност која е имплементирана е филтрирањето според жанрот.
  
  
  ![Watched movies](https://github.com/vojches/MovieTracker/blob/screenshots/screen4.png)
  Со кликнување на еден од филмовите во листата, на десната страна на прозорецот се прикажуваат детали. Имплементирано е копче за бришење на филмови од листата, во случај ако корисникот внел погрешен филм.
  
  
  Во третиот таб “Watchlist” има преглед на филмовите кои корисникот планира да ги гледа. Функционалностите кои се овозможени за гледаните филмови во вториот таб, ги имплементиравме и овде. Во овој таб има уште едно копче за додавање во листата со гледани филмови, во случај да корисникот го гледал филмот кој го внел во листата Movies – to – watch.
  ![Watchlist](https://github.com/vojches/MovieTracker/blob/screenshots/screen5.png)
  
  Во случај корисникот да има проблеми со Интернет конекцијата, пребарувањето на филмови ќе биде оневозможено, односно сите копчиња поврзани со селекција на филмови и приказ на детали ќе бидат disabled. Сепак, корисникот повторно ќе може да ги прегледува листите со филмови и ќе може да префрла, како и брише филмови од истите.
  
  ![Watchlist](https://github.com/vojches/MovieTracker/blob/screenshots/screen7.png)

### 3.	Претставување на решението

#### 3.1 Опис на решението

MovieTracker апликацијата се заснова на класата Movie, каде се чуваат сите податоци за филмовите, како и некои методи кои се користат за приказ на атрибутите. Врз основа на оваа класа и класата Genre е креирана базата на податоци, при што за нејзино креирање е искористен Entity Framework заради олеснително мапирање и креирање на табелите.

	public partial class Movie
	{
	        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
	        public Movie()
	        {
	            this.Genres = new HashSet<Genre>();
	        }
	    
	        public int Id { get; set; }
	        public string ImdbID { get; set; }
	        public string Title { get; set; }
	        public int Year { get; set; }
	        public int Runtime { get; set; }
	        public string Director { get; set; }
	        public string Actors { get; set; }
	        public string Plot { get; set; }
	        public string Awards { get; set; }
	        public string Language { get; set; }
	        public string Image { get; set; }
	        public decimal Rating { get; set; }
	        public Nullable<int> Type { get; set; }
	        public Nullable<System.DateTime> Release { get; set; }
	    
	        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
	        public virtual ICollection<Genre> Genres { get; set; }
	}

Дополнително освен основните податоците за филмот, се чува и Type атрибут, чии вредности (null, 0, 1, 2) соодветно ни укажуваат дали филмот го нема воопшто во базата на податоци, дали го има, но не е ниту гледан, ниту би сакале да го гледаме во иднина (претходно бил избришан од некоја од листите), дали филмот планираме да го гледаме или пак филмот е веќе изгледан.

Исто така, во проектот е креирана Data Access Layer класа (DAMovie) којa ни овозможува полесен пристап до базата на податоци како и реискористливост на дефинираните функции на повеќе места во апликацијата.

#### 3.2 Опис на функција

Оваа функција се користи за да се повлечат деталните податоци за филмот од веб сервисот кои подоцна ќе се запишат во базата. Пред да се повлечат овие детални податоци, апликацијата има основни информации за филмот кои се преземаат кога корисникот пребарува одреден наслов.

Функцијата povleciDetalniPodatoci на влез прима аргумент string кој подоцна се користи за повик до веб сервисот. Прво се иницијализира објект од класата WebClient, а потоа со објект од оваа класа се повикува методот DownloadString каде што се внесува url адреса од каде што треба да се повлечат податоците.

Податоците се преземаат во json формат и подоцна се парсираат, се прават сите проверки и вредностите се запишуваат во објектот curr од тип SearchMovie. Објектот curr е објект кој го претставува селектираниот филм од листата во која се прикажани филмовите кои одговараат на клучбниот збор по кој пребарува корисникот.

	private void povleciDetalniPodatoci(string id)
	{
            WebClient c = new WebClient();
            string data = c.DownloadString("http://www.omdbapi.com/?i=" + id + "&plot=full&r=json");
            JObject o = JObject.Parse(data);
            DateTime released;
            if (!o["Released"].ToString().Equals("N/A"))
            {
                string date = o["Released"].ToString();
                string[] d = date.Split(' ');
                int godina = 0;
                int.TryParse(d[2], out godina);
                int den = 0;
                int.TryParse(d[0], out den);

                released = new DateTime(godina, mesec(d[1]), den);
            }
            else
            {
                released = new DateTime(1800, 01, 01);
            }    

            List<string> genres = new List<string>();

            string[] gen2 = o["Genre"].ToString().Split(',');
            foreach (string i in gen2)
            {
                genres.Add(i.Trim());
            }

            string[] runtimeA = o["Runtime"].ToString().Split(' ');
            int runtime = 0;
            int.TryParse(runtimeA[0], out runtime);
            float imdbRating = 0;
            if (!o["imdbRating"].ToString().Equals("N/A"))
            {
                imdbRating = (float)o["imdbRating"];
            }
                        
            curr.release = released;
            curr.runtime = runtime;
            curr.genres = genres;
            curr.director = o["Director"].ToString();
            curr.actors = o["Actors"].ToString();
            curr.plot = o["Plot"].ToString();
            curr.language = o["Language"].ToString();
            curr.awards = o["Awards"].ToString();
            curr.imdbRating = imdbRating;            
	}
        
#### 3.3	Упатство за користење на апликацијата

Бидејќи апликацијата е поврзана со база на податоци, мора да имате инсталирано SQL Server, заедно со LocalDB и да направите конекција со истата, како би можеле да ја извршите апликацијата.

![Connection](https://github.com/vojches/MovieTracker/blob/screenshots/screen8.png)

Откако ќе кликнете на копчето Add Connection во Server Explorer, ќе ви се отвори нов прозорец каде треба да го внесете името на локалниот сервер, како и името на базата која што сакате да ја креирате. Во нашиот случај, името на базата МОРА да биде Movie.

![Database](https://github.com/vojches/MovieTracker/blob/screenshots/screen9.png)

Базата треба да ви се појави во Server Explorer прозорецот, што означува успешно креирање на истата. За да ги генерирате потребните табели како апликацијта би функционирала, потребно е да jа извршите скриптата под име Model1.edmx.sql која се наоѓа во MovieTracker фолдерот.

![Query](https://github.com/vojches/MovieTracker/blob/screenshots/screen10.png)

Откако ќе кликнете на Execute копчето, ќе ви се испише порака „Query executed successfully“ доколку сè поминало во најдобар ред. :)

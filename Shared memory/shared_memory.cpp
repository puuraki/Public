/*
Tee sovellus, jossa prosessi luo fork-kutsulla lapsiprosessin. Prosesseille tehdään yhteinen jaettu muisti. 
Emoprosessi toimii sovelluksen käyttöliittymänä ja lapsiprosessi toimii sovellusprosessina. 
Emoprosessi lukee kokonaisluvun ja välittää sen lapsiprosessille, joka laskee luvun kertoman ja välittää sen takaisin emoprosessille. 
Emoprosessi tulostaa sitten tuloksen. Prosessit toimivat määrittelyn mukaan, kunnes annetaan negatiivinen kokonaisluku, 
jolloin molemmat prosessit lopetetaan. Emoprosessi pyörii käyttöliittymäsilmukassa, kunnes lopettaa toiminnan. 
Lapsiprosessi pyörii sovellussilmukassa, kunnes lopettaa toimintansa. Prosessien synkronointiin voit käyttää semaforeja.

g++ -o filename filename.cpp -lpthread -lrt
*/

#include <fcntl.h>
#include <iostream>
#include <semaphore.h>
#include <sstream>
#include <sys/mman.h>
#include <wait.h> 
 
 using namespace std;
 
typedef struct {
    sem_t one;
    sem_t two;
} semPair;

struct region {
    int value;
};
 
int main(int argc, const char *argv[]) {
	string input = "";
	int myNumber = 0;
	int total = 1;
	struct region *rptr;
	bool isRunning = true;
	
    int shm = shm_open("semaphore", O_CREAT | O_RDWR, S_IRUSR | S_IWUSR);
	int shm2 = shm_open("sharedVal", O_CREAT | O_RDWR, S_IRUSR | S_IWUSR);
    ftruncate(shm, sizeof(sem_t));
	ftruncate(shm2, sizeof(struct region));
    semPair *sem = (semPair *)mmap(NULL, sizeof(sem_t), PROT_READ | PROT_WRITE, MAP_SHARED, shm, 0);
	rptr = (region *)mmap(NULL, sizeof(struct region), PROT_READ | PROT_WRITE, MAP_SHARED, shm2, 0);
 
    sem_init(&(sem->one), 1, 0);
    sem_init(&(sem->two), 1, 0);
	
	rptr->value = 1;
	
	pid_t pid = fork();
	
	/* ----- CHILD PROCESS ----- */
	
	if (pid == 0) {
		while(isRunning){
			sem_wait(&(sem->two));
			
			cout <<  "CHILD: " << endl;
			total = 1;
			
			if(rptr->value < 0)
			{
				isRunning = false;
				cout<< "Closing child."<< endl;
			}
			
			if(rptr->value >= 0)
			{
				cout<<  "Calculating..." << endl;
			
				for (int i = 1; i <= rptr->value; i++)
				{
					total = total*i;
				}
				
				rptr->value = total;
			
				cout << "Calculation ready." << endl;
			}
			sem_post(&(sem->one));
		}
	} 

	/* ---- PARENT PROCESS ----- */
	
	else {
		while(isRunning){
			cout <<  "PARENT: " << endl;
			cout << "Please enter a valid number: " << endl;
			
			getline(cin, input);
			stringstream ss(input);
			ss >> myNumber;
			
			ss.str("");
			ss.clear();
			
			rptr->value = myNumber;
			
			if(rptr->value >= 0)
			{
				cout << "Value sent to child process for calculation." << endl;
			}
			
			sem_post(&(sem->two));
			sem_wait(&(sem->one));
				
			if(rptr->value >= 0)
			{
				cout<< "Factorial of " << myNumber << " is "<< rptr->value << endl;
			}
			
			if(rptr->value < 0)
			{
				isRunning = false;
			}
			
			cout<< endl;
		}
		cout<< "Waiting for child to close."<< endl;;
		
		waitpid(pid, NULL, 0);
		cout<< "Child closed. Exiting program." << endl;
	}
    return 0;
}
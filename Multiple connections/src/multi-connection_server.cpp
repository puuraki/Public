/*
Tee Linux laskinpalvelin, joka pitää useita samanaikaisia yhteyksiä 
auki niin kauan kuin asiakkaat ovat käynnissä. Tee myös windows 
käyttöliittymäasiakas, joka käyttää palvelinta. Laskimella on kaksi 
operaatiota: yhteenlasku ja vähennyslasku.

g++ -o filename filename.cpp -lpthread -lrt
*/

#include <fcntl.h>
#include <iostream>
#include <netinet/in.h>
#include <semaphore.h>
#include <sstream>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/mman.h>
#include <wait.h> 
 
 using namespace std;
 
typedef struct {
    sem_t one;
    sem_t two;
} semPair;

struct region {
    bool isRunning;
};
 
int main(int argc, const char *argv[]) {
	
	// define variables
	string input = "";
	int myNumber = 0;
	int total = 1;
	struct region *rptr;
	
	string strFirstValue = "";
	string strSecondValue = "";
	string calcOperator = "";
	
	int firstValue, secondValue, valueSlot, result;
	
	// create shared memory and insert semaphores and a shared value in it
    int shm = shm_open("semaphore", O_CREAT | O_RDWR, S_IRUSR | S_IWUSR);
	int shm2 = shm_open("sharedVal", O_CREAT | O_RDWR, S_IRUSR | S_IWUSR);
    ftruncate(shm, sizeof(sem_t));
	ftruncate(shm2, sizeof(struct region));
    semPair *sem = (semPair *)mmap(NULL, sizeof(sem_t), PROT_READ | PROT_WRITE, MAP_SHARED, shm, 0);
	rptr = (region *)mmap(NULL, sizeof(struct region), PROT_READ | PROT_WRITE, MAP_SHARED, shm2, 0);
 
	// initialize semaphores
    sem_init(&(sem->one), 1, 1);
    sem_init(&(sem->two), 1, 1);
	
	rptr->isRunning = true;
	
	// define variables used for networking functions
	struct sockaddr_in serv,client;
	char buffer[256];
	int sock_desc, conn_desc, n, length;
	socklen_t size;
	int opt = 1;
	string msg;
	stringstream ss;
	
	// create socket and bind it
	sock_desc = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	bzero((char *)&serv, sizeof(serv));
	
	serv.sin_family = AF_INET;
	serv.sin_port = htons(4360);
	serv.sin_addr.s_addr = INADDR_ANY;
	
	if (bind(sock_desc, (sockaddr *) &serv, sizeof(serv)) < 0 )
	{
		printf("Failed to bind\n");
		return 0;
	}
	
	if(listen(sock_desc, 5) == -1)
	{
		printf("Failed to listen\n");
	}
	cout << "Listening for connections to port 4360" << endl;
	
	if (setsockopt(sock_desc, SOL_SOCKET, SO_REUSEADDR, &opt, sizeof(int)) == -1)
	{
		printf("setsockopt") ;
		return 0;
	}
	
	int pid;
	
	while(rptr->isRunning)
	{
		conn_desc = accept(sock_desc, (struct sockaddr *)&client, &size);							// The program waits for new connections
		if(conn_desc == -1)																			// and if a connection is successfully accepted
		{																							// the program will proceed to call fork().
			printf("Connection failed\n");
			return 0;
		}
		
		sem_wait(&(sem->one));
		sem_wait(&(sem->two));
		
		/* ---- FORK() ----- */
		if((pid=fork()) == -1){
			close(conn_desc);
			close(sock_desc);
			rptr->isRunning = false;
			return 0;
		}
	
		/* ---- PARENT PROCESS ----- */
		else if(pid > 0){
			cout<<" PARENT: "<<endl;																// Parent closes the connection to let
			close(conn_desc);																		// the child take full control of the socket.
			cout << "Should the program continue? ('no' will end the program): ";					// At this point the user can choose if the program
			string answer;																			// should continue.
			cin >> answer;
			if (answer == "no")
			{
				rptr->isRunning = false;
			}
			if(!rptr->isRunning){
				cout<< "Waiting for children to close."<< endl;;
				
				waitpid(-1, NULL, 0);
				cout<< "Children closed. Exiting program." << endl;
				return 0;
			}
			sem_post(&(sem->two));
		}
		
		
		/* ----- CHILD PROCESS ----- */
		else if (pid == 0){
			
			cout << "CHILD: " << endl;
			
			size = sizeof(client);
			
			sem_post(&(sem->one));
			
			/* ----- CHILD PROGRAM LOOP ----- */
			while(rptr->isRunning){
				
				n = read(conn_desc,buffer,255);
				
				if (n < 0)
				{
					printf("ERROR reading from socket");
					close(conn_desc);
					close(sock_desc);
					return 0;
				}
				else if (n > 0)
				{
					msg = string(buffer);
					length = msg.size();
					
					if (length == 0)
					{
						close(conn_desc);
						close(sock_desc);
						return 0;
					}
					
					cout<<"\nReceived message: "<<msg<<endl;
					valueSlot = 1;
					strFirstValue = "";
					strSecondValue = "";
					int count = 0;
					
					for (int i = 0; i < msg.length(); i++)											// Child goes through the received message and
					{																				// sets the first and second values based on the
						if(valueSlot == 1 && (count == 0 || (msg[i] != '-' && msg[i] != '+')))		// given rules.
						{
							strFirstValue += msg[i];
							count++;
						}
						else if(valueSlot == 2 && (count == 0 || (msg[i] != '-' && msg[i] != '+')))
						{
							strSecondValue += msg[i];
							count++;
						}
						else if(msg[i] == '+' && count > 0)
						{
							calcOperator = '+';
							valueSlot = 2;
							count = 0;
						}
						else if(msg[i] == '-' && count > 0)
						{
							calcOperator = '-';
							valueSlot = 2;
							count = 0;
						}
						else
						{
							printf("Message error");
						}

					}
					firstValue = atoi(strFirstValue.c_str());										// Convert strings of first and second values
					secondValue = atoi(strSecondValue.c_str());										// to integers.
					
					if(calcOperator == "+")
					{
						cout << calcOperator << endl;
						result = firstValue + secondValue;
						cout << result << endl;
					}
					else
					{
						cout << calcOperator << endl;
						result = firstValue - secondValue;
						cout << result << endl;
					}
					
					msg = "The result is: ";
					ss << result;
					msg += ss.str();
					cout << msg << endl;
					
					ss.str("");
					ss.clear();
					
					strncpy(buffer, msg.c_str(), sizeof(buffer));
						
					n = write(conn_desc, buffer,255);
					if(n < 0)
						printf("ERROR WRITING TO SOCKET");
					
					n = 0;
				}
			}
			close(conn_desc);
			close(sock_desc);
			return 0;
		}
	}
	close(sock_desc);
	return 0;
}
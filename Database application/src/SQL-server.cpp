/*
	g++ SQL-server.cpp -o SQL-server `mysql_config --include --libs`
*/

#include <algorithm>
#include <cstring>
#include <iostream>
#include <mysql/mysql.h>
#include <netinet/in.h>
#include <sstream>
#include <stdio.h>
#include <stdlib.h>
#include <string>
#include <sys/socket.h>
#include <sys/types.h>
#include <unistd.h>
#include <vector>

using namespace std;

string selSwitch(int value, string selection);
void parseUserInfo(string info);
bool connect();
void disconnect();
string showTables();
string select();

string selectedTable = "";

MYSQL_RES *result;
MYSQL_ROW row;
MYSQL_FIELD *mysqlFields;
MYSQL *connection, mysql;
int state;

string server = "";
string username = "";
string password = "";
string database = "";
int port = 3306;

int sock_desc, conn_desc;
bool isRunning = true;

int main ()
{
	// Set TCP variables
	struct sockaddr_in serv,client;
	char buffer[2048];
	int n, length;
	socklen_t size;
	int opt = 1;
	string msg;
	stringstream ss;
	
	sock_desc = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	bzero((char *)&serv, sizeof(serv));
	
	serv.sin_family = AF_INET;
	serv.sin_port = htons(4360); // The port can be changed here
	serv.sin_addr.s_addr = INADDR_ANY;
	
	// Set SQL variables
	bool isConnected = false;
	
	int command;
	string selection;
	
	if (bind(sock_desc, (sockaddr *) &serv, sizeof(serv)) < 0 )
	{
		printf("Failed to bind\n");
		return 0;
	}
	
	listen(sock_desc, 5);
	
	printf("Waiting for connection.. \n");
	
	size = sizeof(client);
	
	if (setsockopt(sock_desc, SOL_SOCKET, SO_REUSEADDR, &opt, sizeof(int)) == -1)
	{
          printf("setsockopt") ;
          return 0;
    }
	
	conn_desc = accept(sock_desc, (struct sockaddr *)&client, &size);
	if(conn_desc == -1)
	{
		printf("Connection failed\n");
		return 0;
	}
	else
		printf("Connection succesful\n");
	
	while (isRunning){
		n = read(conn_desc,buffer,255);
		
		if (n < 0)
		{
			printf("ERROR reading from socket");
			isRunning = false;
			close(conn_desc);
			close(sock_desc);
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
			
			ss << msg[0];
			ss >> selection;
			ss.str("");
			ss.clear();
			
			command = atoi(selection.c_str());
			selection = "";
			
			if (msg.length() > 2)
			{
				for (int i = 2; i < msg.length(); i++)
				{
					ss << msg[i];
				}
				ss >> selection;
				ss.str("");
				ss.clear();
			}
			
			msg = "";
			msg = selSwitch(command, selection);
			if(!isRunning)
			{
				break;
			}
			int pos1 = 0;
			int pos2 = 2040;
			int msgLength = 0;
			int packets = 1;
			if(msg.length() != 0)
			{
				msgLength = msg.length();
				packets = (msgLength/2040)+1;
			}
			
			string msgSend = "";
			
			ss << packets;
			ss >> msgSend;
			ss.str("");
			ss.clear();
			
			msgSend += "|";
			msgSend += msg;

			if (packets > 0)
			{
				for (int i = 0; i < packets; i++)
				{
					strcpy(buffer,"");
					memset(buffer, 0, sizeof(buffer));
					msgSend.copy(buffer, pos2-pos1, pos1);
					buffer[2040] = '|';
					n = write(conn_desc, buffer, 2048);
					if(n < 0)
						printf("ERROR WRITING TO SOCKET");
					
					n = 0;
					pos1 = pos2+1;
					pos2 += pos2+1;
				}
			}
			else
			{
				strcpy(buffer, "");
				memset(buffer, 0, sizeof(buffer));
				strncpy(buffer, msg.c_str(), sizeof(buffer));
				printf(buffer);
				n = write(conn_desc, buffer, 2048);
				if(n < 0)
					printf("ERROR WRITING TO SOCKET");
				
				n = 0;
			}
			//strncpy(buffer, msg.c_str(), sizeof(buffer));
			
		}
	}
	close(conn_desc);
	close(sock_desc);
	return 0;
}

string selSwitch(int value, string data)
{
	string resultStr = "";
	switch (value)
	{
		case 0:
		{
			// CONNECT
			cout << "CONNECT SQL" << endl;
			parseUserInfo(data);
			if(connect())
			{
				cout << "Database connection successful!" << endl;
			}
			break;
		}
		case 1:
		{
			// SELECT TABLE
			cout << "SELECT TABLE" << endl;
			selectedTable = data;
			resultStr = select();
			break;
		}
		case 2:
		{
			// PRINT ALL TABLES
			cout << "PRINT ALL TABLES" << endl;
			resultStr = showTables();
			break;
		}
		case 3:
		{
			// EXIT
			cout << "EXIT" << endl;
			disconnect();
			isRunning = false;
			break;
		}
		default:
		{
			cout << "Invalid selection!" << endl;
			break;
		}
	}
	return resultStr;
}

bool connect()
{
	mysql_init(&mysql);
    connection = mysql_real_connect(&mysql,server.c_str(),username.c_str(),password.c_str(),database.c_str(), port, 0, 0);
    
    if (connection == NULL)
    {
        printf(mysql_error(&mysql));
        return false;
    }
	else
	{
		return true;
	}
	
}

void parseUserInfo(string info)
{
	int counter = 0;
	int pos1 = 0;
	int pos2 = -1;
	
	for (int i = 0; i < info.length(); i++)
	{	
		if (info[i] == '&')
		{
			if (pos1 != -1)
			{
				pos2 = i;
			}
			if (pos1 != -1 && pos2 != -1)
			{
				if (counter == 0)
				{
					server.assign(info, pos1, pos2-pos1);
				}
				else if (counter == 1)
				{
					username.assign(info, pos1, pos2-pos1);
				}
				else if (counter == 2)
				{
					password.assign(info, pos1, pos2-pos1);
				}
				pos1 = pos2+1;
				pos2 = -1;
			}
			counter++;
		}
		else if (i+1 == info.length())
		{
			database.assign(info, pos1, i);
		}
	}
}

void disconnect()
{
    mysql_close(connection);
	close(conn_desc);
	close(sock_desc);
	isRunning = false;
};

string showTables()
{
	int count = 0;
	string allTables;
	char tempStr[2048];
	
	strcpy(tempStr, "");
	state = mysql_query(connection, "SHOW TABLES");
	if (state != 0)
	{
		printf(mysql_error(connection));
	}
	result = mysql_store_result(connection);
	while ( (row=mysql_fetch_row(result)) != NULL )
    {
		sprintf(tempStr + strlen(tempStr), "%s&", (row[0] ? row[0] : "NULL"));
		count++;
    }
	allTables = string(tempStr);
	mysql_free_result(result);
	return allTables;
}

string select()
{
	string cellStr;
	char cells[65535];
	char newData[2048];
	char sqlCmd[256];
	strcpy(cells,"");
	strcpy(newData,"");
	strcpy(sqlCmd,"");

	sprintf(sqlCmd, "SELECT * FROM %s", selectedTable.c_str());
	
    state = mysql_query(connection, sqlCmd);
    if (state != 0)
    {
        printf(mysql_error(connection));
    }
    result = mysql_store_result(connection);
	int rowNum, fieldNum, length = 2;
	
	if (result) 
	{
		rowNum = mysql_num_rows(result);
		fieldNum = mysql_num_fields(result);
		mysqlFields = mysql_fetch_fields(result);
	}
	else
	{
		rowNum = 0;
		fieldNum = 0;
		mysqlFields = NULL;
	}
	
	string dataArr[rowNum+1][fieldNum];
	
	// Get column names
	for (int j = 0; j < fieldNum; j++)
	{
		dataArr[0][j] = mysqlFields[j].name;
	}
	
	int count = 0;
	
	// Get field values
    while ( (row=mysql_fetch_row(result)) != NULL )
    {
		for (int i = 0; i < fieldNum; i++)
		{
			dataArr[count+1][i] = row[i] ? row[i] : "NULL";
		}
		count++;
    }
	
	// Store table data in a string
	//if (sizeof(dataArr) < 2048)
	//{
		for (int k = 0; k < count; k++)
		{
			for (int i = 0; i < fieldNum; i++)
			{
				if (i == 0 && k != 0)
				{
					sprintf(cells + strlen(cells), "&%s;", dataArr[k][i].c_str());
				}
				else if(i == fieldNum - 1)
				{
					sprintf(cells + strlen(cells), "%s", dataArr[k][i].c_str());
				}
				else
				{
					sprintf(cells + strlen(cells), "%s;", dataArr[k][i].c_str());
				}
			}
		}
	//}
	//else
	//{
	//	printf("Table is too big");
	//}
	
	cellStr = string(cells);
    mysql_free_result(result);
	return cellStr;
}
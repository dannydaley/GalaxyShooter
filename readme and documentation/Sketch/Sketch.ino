// Uduino Default Board
#include<Uduino.h>
Uduino uduino("ControlDeck");
// Setup Sonar
#include <NewPing.h> //To add the library : Sketch>Include Library>Manager Libraries...> [search and add NewPing library] 
#define TRIGGER_PINL  9  
#define ECHO_PINL     10  
#define TRIGGER_PINR  5
#define ECHO_PINR  6
#define MAX_DISTANCE 300 
NewPing sonarL(TRIGGER_PINL, ECHO_PINL, MAX_DISTANCE);
NewPing sonarR(TRIGGER_PINR, ECHO_PINR, MAX_DISTANCE); 

int BUTTONL = 7;
int BUTTONR = 8;
// Servo
#include <Servo.h>
#define MAXSERVOS 8


void setup()
{
  Serial.begin(9600);

#if defined (__AVR_ATmega32U4__) // Leonardo
  while (!Serial) {}
#elif defined(__PIC32MX__)
  delay(1000);
#endif
  pinMode(BUTTONL, INPUT);
  pinMode(BUTTONR, INPUT);
  uduino.addCommand("s", SetMode);
  uduino.addCommand("d", WritePinDigital);
  uduino.addCommand("a", WritePinAnalog);
  uduino.addCommand("rd", ReadDigitalPin);
  uduino.addCommand("r", ReadAnalogPin);
  uduino.addCommand("br", BundleReadPin);
  uduino.addCommand("b", ReadBundle);
  uduino.addInitFunction(InitializeServos);
  uduino.addDisconnectedFunction(DisconnectAllServos);
  uduino.addInitFunction(DisconnectAllServos);
}

void ReadBundle() {
  char *arg;
  char *number;
  number = uduino.next();
  int len ;
  if (number != NULL)
    len = atoi(number);
  for (int i = 0; i < len; i++) {
    uduino.launchCommand(arg);
  }
}

void SetMode() {
  int pinToMap;
  char *arg;
  arg = uduino.next();
  if (arg != NULL)
  {
    pinToMap = atoi(arg);
  }
  int type;
  arg = uduino.next();
  if (arg != NULL)
  {
    type = atoi(arg);
    PinSetMode(pinToMap, type);
  }
}

void PinSetMode(int pin, int type) {
  //TODO : vérifier que ça, ça fonctionne
  if (type != 4)
    DisconnectServo(pin);

  switch (type) {
    case 0: // Output
      pinMode(pin, OUTPUT);
      break;
    case 1: // PWM
      pinMode(pin, OUTPUT);
      break;
    case 2: // Analog
      pinMode(pin, INPUT);
      break;
    case 3: // Input_Pullup
      pinMode(pin, INPUT_PULLUP);
      break;
    case 4: // Servo
      SetupServo(pin);
      break;
  }
}

void WritePinAnalog() {
  int pinToMap;
  char *arg;
  arg = uduino.next();
  if (arg != NULL)
  {
    pinToMap = atoi(arg);
  }

  int value;
  arg = uduino.next();
  if (arg != NULL)
  {
    value = atoi(arg);

    if (ServoConnectedPin(pinToMap)) {
      UpdateServo(pinToMap, value);
    } else {
      analogWrite(pinToMap, value);
    }
  }
}

void WritePinDigital() {
  int pinToMap;
  char *arg;
  arg = uduino.next();
  if (arg != NULL)
  {
    pinToMap = atoi(arg);
  }
  int value;
  arg = uduino.next();
  if (arg != NULL)
  {
    value = atoi(arg);
    digitalWrite(pinToMap, value);
  }
}

void ReadAnalogPin() {
  int pinToRead;
  char *arg;
  arg = uduino.next();
  if (arg != NULL)
  {
    pinToRead = atoi(arg);
    printValue(pinToRead, analogRead(pinToRead));
  }
}

void ReadDigitalPin() {
  int pinToRead;
  char *arg;
  arg = uduino.next();
  if (arg != NULL)
  {
    pinToRead = atoi(arg);
  }
  printValue(pinToRead, digitalRead(pinToRead));
}

void BundleReadPin() {
  int pinToRead;
  char *arg;
  arg = uduino.next();
  if (arg != NULL)
  {
    pinToRead = atoi(arg);
  }
  printValue(pinToRead, analogRead(pinToRead));
}

Servo myservo;
void loop()
{
    uduino.update(); // This part is needed to be detected by Unity. 
  
  if (uduino.isConnected()) {
    if (digitalRead(BUTTONL) > 0) {   
    Serial.println("BL");
    }
    if (digitalRead(BUTTONR) > 0){
    Serial.println("BR");
    }
    Serial.print("L");
   Serial.println(sonarL.ping_cm()); 
   uduino.delay(30);
   Serial.print("R");
   Serial.println(sonarR.ping_cm());
   uduino.delay(30);
  }
}

void printValue(int pin, int value) {
  Serial.print(pin);
  Serial.print(" ");
  Serial.println(value);
  // TODO : Here we could put bundle read multiple pins if(Bundle) { ... add delimiter // } ...
}

/* SERVO CODE */



Servo servos[MAXSERVOS];
int servoPinMap[MAXSERVOS];

void InitializeServos() {
  //Serial.println("Init all servos");
  for (int i = 0; i < MAXSERVOS - 1; i++ ) {
    servoPinMap[i] = -1;
    servos[i].detach();
  }
}

void SetupServo(int pin) {
  if (ServoConnectedPin(pin))
    return;

  int nextIndex = GetAvailableIndexByPin(-1);
  if (nextIndex == -1)
    nextIndex = 0;

  //Serial.print("Starting servo on ");
  //Serial.print(pin);
  //Serial.print(" index ");
  //Serial.println(nextIndex);
  servoPinMap[nextIndex] = pin;
  servos[nextIndex].attach(pin);
}


void DisconnectServo(int pin) {
  servos[GetAvailableIndexByPin(pin)].detach();
  servoPinMap[GetAvailableIndexByPin(pin)] = -1;
}

bool ServoConnectedPin(int pin) {
  if (GetAvailableIndexByPin(pin) == -1) return false;
  else return true;
}



int GetAvailableIndexByPin(int pin) {
  for (int i = 0; i < MAXSERVOS - 1; i++ ) {
    //Serial.print("Finding pin  ");
    //Serial.print(pin);
    //Serial.print(" in ");
    //Serial.print(i);
    //Serial.print(" value is  ");
    //Serial.print(servoPinMap[i]);
    if (servoPinMap[i] == pin) {
      //Serial.println(" -> Found");
      return i;
    }
    //Serial.println();
  }
  return -1; // return first index, but this should not happend
}


void UpdateServo(int pin, int value) {
  int index = GetAvailableIndexByPin(pin);
  //Serial.print("Updating servo on ");
  //Serial.print(pin);
  //Serial.print(" index ");
  //Serial.print(index);
  //Serial.print(" value ");
  //Serial.println(value);
  servos[index].write(value);
  delay(10);
}

void DisconnectAllServos() {
  for (int i = 0; i < MAXSERVOS; i++) {
    servos[i].detach();
    digitalWrite(servoPinMap[i], LOW);
    servoPinMap[i] = -1;
  }
}

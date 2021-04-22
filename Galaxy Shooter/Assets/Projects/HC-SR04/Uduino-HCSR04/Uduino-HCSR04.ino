// Setup Uduino
#include <Uduino.h>
Uduino uduino("Range");

// Setup Sonar
#include <NewPing.h> //To add the library : Sketch>Include Library>Manager Libraries...> [search and add NewPing library] 
#define TRIGGER_PIN  12  
#define ECHO_PIN     11  
#define MAX_DISTANCE 300 
NewPing sonar(TRIGGER_PIN, ECHO_PIN, MAX_DISTANCE); 

void setup() {
  Serial.begin(9600);
}

void loop() {
  uduino.update();
  if (uduino.isConnected()) {
    Serial.println(sonar.ping_cm()); 
  }
  delay(30);
}

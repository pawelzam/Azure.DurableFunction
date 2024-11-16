# DurableFunction Project

This project demonstrates an Azure Durable Function implementation for orchestrating state transitions of an order through different stages (`Created`, `Paid`, `Delivered`, `Invoiced`).

---

## **Features**
1. **Orchestrator Function**  
   Manages the flow of the order state by calling activity functions in sequence.
2. **Activity Function**  
   Processes and updates the state of the order.
3. **HTTP Trigger**  
   Starts the orchestration process via an HTTP request.

---

## **Project Structure**
- **Orchestrator Function (`RunOrchestrator`)**  
  Coordinates the order's state transitions by invoking the activity function three times.
  
- **Activity Function (`ProcessOrder`)**  
  Handles the state update for the order based on its current state.

- **HTTP Trigger (`HttpStart`)**  
  Initiates the orchestrator and provides an instance management endpoint for checking the orchestration status.

---

## **Order Workflow**
1. The order begins in the `Created` state.
2. Transitions:
   - **Created** → **Paid**
   - **Paid** → **Delivered**
   - **Delivered** → **Invoiced**
3. Each transition is logged for tracking the state change.

---

## **How to Use**

### **Prerequisites**
- Azure Functions Core Tools installed
- .NET 6 SDK installed
- Azure Storage Emulator or Azure Storage account

### **Running Locally**
1. Clone this repository.
2. Install dependencies using `dotnet restore`.
3. Start the function app locally:
   ```bash
   func start
   ```
4. Trigger the function by sending a GET or POST request to:
   ```
   http://localhost:{port}/api/Function_HttpStart
   ```

   Replace `{port}` with the port number displayed when running the function app.

5. Monitor the orchestration status using the URL returned in the response.

---

## **Technologies Used**
- **Azure Functions**
- **Durable Task Framework**
- **.NET 6**
- **Azure Storage (for orchestration state management)**

---

## **Endpoints**
- **Start Orchestration**  
  - URL: `/api/Function_HttpStart`
  - Methods: `GET`, `POST`

---

## **Logging**
The project leverages `ILogger` to log:
- State transitions of the order.
- Orchestration start and status.

---

## **Example Output**
1. **HTTP Trigger Response:**
   ```json
   {
     "id": "instanceId",
     "statusQueryGetUri": "https://...",
     "sendEventPostUri": "https://...",
     "terminatePostUri": "https://...",
     "purgeHistoryDeleteUri": "https://..."
   }
   ```

2. **Log Messages:**
   ```
   Processing order {Id}
   Order {Id} state changed to {State}.
   Final state of order {Id} is {State}.
   ```

---

Feel free to explore and extend this project to suit your business needs!
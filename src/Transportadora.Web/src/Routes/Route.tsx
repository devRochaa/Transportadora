import { createBrowserRouter } from "react-router-dom";
import TrackingPage from "@/Pages/TrackingPage/TrackingPage";
import App from "@/App";

export const router = createBrowserRouter([
  {
    element: <App />,
    children: [
      {
        path: "/",
        element: <TrackingPage />,
      },
    ],
  },
]);

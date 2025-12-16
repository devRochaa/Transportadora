import { createBrowserRouter } from "react-router-dom";
import TrackingPage from "@/Pages/TrackingPage/TrackingPage";
import App from "@/App";
import DriversPage from "@/Pages/DriversPage/DriversPage";

export const router = createBrowserRouter([
  {
    element: <App />,
    children: [
      {
        path: "/",
        element: <TrackingPage />,
      },
      {
        path: "/motoristas",
        element: <DriversPage />,
      },
    ],
  },
]);

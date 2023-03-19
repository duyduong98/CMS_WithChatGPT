//import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { TestSignalR } from "./components/TestSignalR";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    requireAuth: false,
    element: <FetchData />
  },
  {
    path: '/category',
    requireAuth: false,
    element: <TestSignalR />
  }
  //...ApiAuthorzationRoutes
];

export default AppRoutes;

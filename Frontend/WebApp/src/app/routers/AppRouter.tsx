import { useTheme } from "../../entities/theme/lib/useTheme"
import clsx from 'clsx'
import { createBrowserRouter, createRoutesFromElements, Outlet, Route, RouterProvider } from "react-router-dom";
import { Layout } from "../layout";
import { LoginPage } from "../../pages/loginPage";

export const AppRouter = () => {
 const {theme} = useTheme();

 const routers = createRoutesFromElements(
  <Route path="/" element={<Layout />} >
    <Route path="login" element={<LoginPage />} />
  </Route>
 );
 const router = createBrowserRouter(routers, {})
 return (
  <div className={clsx('app', theme)} >
    <RouterProvider router={router} />
  </div>
 );
}

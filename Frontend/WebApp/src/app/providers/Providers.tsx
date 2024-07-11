import { FC } from 'react'
import { ErrorBoundary } from 'react-error-boundary';
import { ConfigProvider, theme } from 'antd';
import { LOCAL_STORAGE_THEME_KEY, Theme } from '../../entities/theme/config/themeContext';
import { ThemeProvider } from '../../entities/theme/ui/ThemeProvider';
import { Provider } from 'react-redux';
import store, { persistor } from '../store';
import { PersistGate } from 'redux-persist/integration/react';


interface IProviders {
  /** Content that will be wrapped by providers. */
  readonly children: JSX.Element
}

export const Providers: FC<IProviders> = ({ children }) => {

  /** ant design theming */
  const { defaultAlgorithm, darkAlgorithm } = theme;
  const currentTheme = localStorage.getItem(LOCAL_STORAGE_THEME_KEY) as Theme || Theme.LIGHT;

  return (
    <ErrorBoundary FallbackComponent={() => <h1>Ooops! Something went wrong. :c </h1>}>

      {/* RTK */}
      <Provider store={store}>
        {/* PERSIST STORE */}
        <PersistGate loading={null} persistor={persistor}>
          {/* ANT DESIGN LAYER */}
          <ConfigProvider
            theme={{
              algorithm: currentTheme === Theme.DARK ? darkAlgorithm : defaultAlgorithm,
            }}
          >
            {/* OWN THEME */}
            <ThemeProvider>
              {children}
            </ThemeProvider>
          </ConfigProvider>
        </PersistGate>
      </Provider>
    </ErrorBoundary>
  )
}

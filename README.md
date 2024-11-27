# gh_tcp_conn
grasshopper addon for tcp connection.

# prerequisites
## 環境の準備
参考： https://developer.rhino3d.com/guides/grasshopper/installing-tools-windows/

1. Visual Studio Community(以下vs、Visual Studio Codeでないので注意)を[Microsoft 公式](https://visualstudio.microsoft.com/ja/downloads/)よりダウンロード

1. ワークロードにおいて、デスクトップとモバイルより「.Netデスクトップ開発」にチェックを入れてインストール。一旦起動。

![「.Netデスクトップ開発」にチェックを入れてインストール](./assets/2022-08-16%20165127.png)

1. Grasshopper アドオン開発用の拡張を追加するために一旦「開始する」の下にある「コードなしで続行」をクリック。
拡張機能>拡張機能の管理を開き、右上の検索より「RhinoCommon and Grasshopper templates for Rhino 7」を検索、ダウンロード。ダウンロード後、vsを閉じるとインストールされる。

![RhinoCommon and Grasshopper templates for Rhino 7](./assets/2022-08-16%20165550.png)

以上でこのRepositoryをクローンしたものが利用可能になる。なお、新規にaddonを作成する場合は、以下を行う。
1. vsを起動し、新しいプロジェクトの作成をする際にテンプレートより「Grasshopper Assembly for Rhino 7(C#)」を選択した上で「次へ」をクリック。

1. ダイアログ（次図）にて、作成するAddonに関する情報を入力する。Optionsの「Provide sample code」にチェックを入れると入出力の追加やそこから値を得て処理を行う際の基本的な書き方が分かるようなサンプルのコードが追加されたものが作成される。

![Addonに関する情報を入力するダイアログ](./assets/2022-08-17%20122412.png)

なお、ここで設定したコンポーネント名などは*Component class name*.cs（ここではNyGrasshopperAssemblyComponent.cs）からでも変更できる（次図）。
![コンポーネント名などの情報](./assets/2022-08-17%20122451.png)

## 開発の手順
1. Rhinoを開き、次のコマンドを実行する
```
GrasshopperDeveloperSettings
```
2. 次図のようなダイアログが開くので、「Add Folder」より「*soluton dir*>bin」を追加する。これでここにあるaddon（.ghaファイル）が読み込まれるようになる

![GrasshopperDeveloperSettings](./assets/2022-08-26%20120850.png)

1. vsにてプロジェクト（ソリューション: .sln）を開き実装を行う。主に編集するのは下記。
    * `RegisterInputParams`
    入力の定義
    * `RegisterOutputParams`
    出力の定義
    * `SolveInstance`
    動作のの定義

1. **デバックの開始**（次図）するとRhinoが立ち上がるのでGrasshopperを開く。

![デバックの開始](./assets/2022-08-26%20121538.png)

1. sample.ghは次図のように10個の三次元の点（三次元点群）を送信することになっている。

![sample.gh](./assets/2022-08-26%20122352.png)

1. runのToggleをTrueにすると、この段階では送信された値を受け取るサーバーが起きていないのでc_statより以下のエラーが表示される。

>connection err: System.Net.Sockets.SocketException (0x80004005): 対象のコンピューターによって拒否されたため、接続できませんでした。 127.0.0.1:3141
   場所 System.Net.Sockets.TcpClient..ctor(String hostname, Int32 port)
   場所 TCPCli.Communicate(Byte[]& sendData, Byte[]& rcvData)


1. サーバープログラムである**svr_test.py**を実行すると、三次元点群の平均が**Callbackのfunc**にて計算されて返送される。

1. GHにてToggleをTrueにし直すと次図のようにsvr_test.pyでの計算結果が受け取れているのが確認できる。

![計算結果の受信](./assets/2022-08-26%20122908.png)

1. アドオン側に変更を加えた場合は、次図のようにホットリロードを行うとGH上のコンポーネントを更新できる。

![ホットリロード](./assets/2022-08-26%20123147.png)

1. 他者にアドオンを配布する際は.gha含め必要なものを渡し、special folders>component folderに配置してもらうことになると思われる。このあたりの作法は未調査。

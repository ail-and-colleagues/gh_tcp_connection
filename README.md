# gh_tcp_conn
grasshopper addon for tcp connection.

# prerequisites
## 環境の準備
参考： https://developer.rhino3d.com/guides/grasshopper/installing-tools-windows/

1. Visual Studio Community(以下vs、Visual Studio Codeでないので注意)を[Microsoft 公式](https://visualstudio.microsoft.com/ja/downloads/)よりダウンロード

1. ワークロードにおいて、デスクトップとモバイルより「.Netデスクトップ開発」にチェックを入れてインストール。一旦起動

1. Grasshopper アドオン開発用の拡張を追加するために一旦「開始する」の下にある「コードなしで続行」をクリック。
拡張機能>拡張機能の管理を開き、右上の検索より「RhinoCommon and Grasshopper templates for Rhino 7」を検索、ダウンロード。ダウンロード後、vsを閉じるとインストールされる

以上でこのRepositoryをクローンしたものが利用可能になる。なお、新規にaddonを作成する場合は、以下を行う。
1. vsを起動し、新しいプロジェクトの作成をする際にテンプレートより「Grasshopper Assembly for Rhino 7(C#)」を選択した上で「次へ」をクリック

1. ダイアログ（次図）にて、作成するAddonに関する情報を入力する。Optionsの「Provide sample code」にチェックを入れると入出力の追加やそこから値を得て処理を行う際の基本的な書き方が分かるようなサンプルのコードが追加されたものが作成される。なお、ここで設定したコンポーネント名などは*Component class name*.cs（ここではNyGrasshopperAssemblyComponent.cs）からでも変更できる。

## 開発の手順
1. Rhinoを開き、次のコマンドを実行する
```
GrasshopperDeveloperSettings
```
2. 次図のようなダイアログが開くので、「Add Folder」より「*soluton dir*>bin」を追加する。これでここにあるaddon（.ghaファイル）が読み込まれるようになる

1. 
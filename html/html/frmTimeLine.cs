using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilETWeb.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;


//CREATE PROCEDURE sp_GetHistoricoSolicitud  
//AS  
//BEGIN

//;WITH cte AS (
//    SELECT 0 num
//    UNION ALL
//    SELECT num = cte.num +1 FROM cte WHERE num <20
//)
//SELECT	Descripcion = 'desc'+ CONVERT(VARCHAR(MAX), cte.num),
//        FechaUltAct = GETDATE()- cte.num,
//        EmailUltAct = 'email'+ CONVERT(VARCHAR(MAX), cte.num % 2 )+ '@x.com'
//FROM	cte
//ORDER	BY 
//        cte.num DESC  

//END  


namespace UtilETWeb
{
    public partial class frmTimeLine : Form
    {
        public frmTimeLine()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = Color.White;   
        }
    }
}

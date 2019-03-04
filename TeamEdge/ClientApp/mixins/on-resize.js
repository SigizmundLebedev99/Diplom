var breakpoints = ['xs','sm','md','lg','xl']
export default{
  methods:{
    ofSize(sizes){
      var breakpoint = this.$vuetify.breakpoint.name;
      var index = breakpoints.indexOf(breakpoint);
      var size = sizes[breakpoint];
      if(!size){
        for(var i = index; i >=0;i--){
          size = sizes[breakpoints[i]]
          if(size)
            return size;
        }
      }
      return size;
    }
  }
}
